using Application.Services;
using Data;
using Domain.Model;
using DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

// Cada ejecución utiliza bases desechables independientes de la aplicación.
foreach (bool practicaExistente in new[] { false, true })
{
    string database = "BolsaTrabajo_MigrationChecks_" + Guid.NewGuid().ToString("N");
    var options = new DbContextOptionsBuilder<BolsaTrabajoContext>()
        .UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database={database};Trusted_Connection=True;TrustServerCertificate=True")
        .Options;
    await using var context = new BolsaTrabajoContext(options);
    try
    {
        var migrator = context.GetService<IMigrator>();
        await migrator.MigrateAsync("20261025120000_AgregarCurriculum");
        await context.Database.ExecuteSqlRawAsync("""
            INSERT INTO [TiposOferta] ([Nombre]) VALUES (N'Categoría personalizada');
            INSERT INTO [Ofertas] ([Titulo], [Detalle], [Estado], [EmpresaId], [TipoOfertaId], [TipoVinculo], [FechaDesde], [FechaHasta])
            VALUES (N'Práctica antigua', N'Detalle', N'Activa', 1, 1, N'PracticaProfesional', '20261101', '20261201'),
                   (N'Contradictoria', N'Detalle', N'Activa', 1, 3, N'EmpleoFullTime', '20261101', '20261201'),
                   (N'Personalizada', N'Detalle', N'Activa', 1, 5, N'PrimerEmpleo', '20261101', '20261201');
            """);
        if (practicaExistente)
            await context.Database.ExecuteSqlRawAsync("INSERT INTO [TiposOferta] ([Nombre]) VALUES (N'Práctica profesional');");

        await migrator.MigrateAsync();
        Check(!context.Database.HasPendingModelChanges(), "El snapshot debe coincidir con el modelo.");
        var practica = await context.TiposOferta.SingleAsync(t => t.Nombre == "Práctica profesional");
        Check(practica.Id != 5, "Debe respetar el ID personalizado existente.");
        Check((await context.Ofertas.SingleAsync(o => o.Titulo == "Práctica antigua")).TipoOfertaId == practica.Id,
            "Debe conservar la modalidad práctica profesional.");
        Check((await context.Ofertas.SingleAsync(o => o.Titulo == "Contradictoria")).TipoOfertaId == 3,
            "En las demás discrepancias debe prevalecer el catálogo.");
        Check((await context.Ofertas.SingleAsync(o => o.Titulo == "Personalizada")).TipoOfertaId == 5,
            "Debe mantener las categorías personalizadas.");
        Check(await Scalar("SELECT COUNT(*) AS [Value] FROM [RespaldoVinculosOferta]") == 3,
            "Debe respaldar todas las ofertas previas.");
        Check(await Scalar("SELECT COUNT(*) AS [Value] FROM sys.columns WHERE object_id = OBJECT_ID(N'Ofertas') AND name = N'TipoVinculo'") == 0,
            "Debe eliminar la columna duplicada.");

        var service = new OfertaService(new OfertaRepository(context), new EmpresaRepository(context), new TipoOfertaRepository(context));
        var nueva = await service.AddAsync(new OfertaDTO
        {
            Titulo = "Nueva sin vínculo", Detalle = "Detalle nuevo", Estado = "Activa", EmpresaId = 1,
            TipoOfertaId = practica.Id, FechaDesde = new DateTime(2026, 11, 1), FechaHasta = new DateTime(2026, 12, 1)
        });
        nueva.Titulo = "Nueva actualizada";
        Check(await service.UpdateAsync(nueva), "Debe actualizar sin vínculo.");
        var filtradas = (await service.GetByCriteriaAsync(new OfertaCriteriaDTO { TipoOfertaId = practica.Id })).ToList();
        Check(filtradas.Count == 2 && filtradas.All(o => o.TipoOfertaId == practica.Id), "Debe filtrar por el catálogo único.");
        try
        {
            await new TipoOfertaService(new TipoOfertaRepository(context)).DeleteAsync(practica.Id);
            throw new Exception("Permitió eliminar una categoría en uso.");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("ofertas asociadas")) { }

        context.ChangeTracker.Clear();
        await migrator.MigrateAsync("20261025120000_AgregarCurriculum");
        Check(await Scalar("SELECT COUNT(*) AS [Value] FROM [Ofertas] WHERE [Titulo] = N'Práctica antigua' AND [TipoOfertaId] = 1 AND [TipoVinculo] = N'PracticaProfesional'") == 1,
            "El rollback debe restaurar los valores originales.");
        Check(await Scalar("SELECT COUNT(*) AS [Value] FROM [Ofertas] WHERE [Titulo] = N'Contradictoria' AND [TipoOfertaId] = 3 AND [TipoVinculo] = N'EmpleoFullTime'") == 1,
            "El respaldo debe conservar también las discrepancias.");
        Check(await Scalar("SELECT COUNT(*) AS [Value] FROM [Ofertas] WHERE [Titulo] = N'Nueva actualizada' AND [TipoVinculo] = N'PracticaProfesional'") == 1,
            "El rollback debe admitir las ofertas creadas después del cambio.");
        await migrator.MigrateAsync();
        Check(await context.Ofertas.CountAsync() == 4, "Debe permitir volver a aplicar la migración.");
        Console.WriteLine($"OK: migración, catálogo {(practicaExistente ? "existente" : "nuevo")}, CRUD, filtros y rollback.");
    }
    finally
    {
        await context.Database.EnsureDeletedAsync();
    }

    async Task<int> Scalar(string sql) => await context.Database.SqlQueryRaw<int>(sql).SingleAsync();
}

static void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
}
