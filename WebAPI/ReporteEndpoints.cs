using Application.Services;
using System.Security.Claims;

namespace WebAPI
{
    public static class ReporteEndpoints
    {
        public static void MapReporteEndpoints(this WebApplication app)
        {
            // La empresa solo ve los datos de sus propias ofertas; el administrador ve todo.
            app.MapGet("/reportes/postulaciones-por-oferta",
                async (DateTime? desde, DateTime? hasta, IReporteService reporteService, ClaimsPrincipal user) =>
                {
                    int? empresaId = user.EsEmpresa() ? (user.GetEmpresaId() ?? 0) : null;

                    try
                    {
                        var filas = await reporteService.GetPostulacionesPorOfertaAsync(empresaId, desde, hasta);
                        return Results.Ok(filas);
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(new { error = ex.Message });
                    }
                })
            .WithName("ReportePostulacionesPorOferta")
            .WithTags("Reportes")
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);

            app.MapGet("/reportes/ofertas-por-tipo", async (IReporteService reporteService, ClaimsPrincipal user) =>
            {
                int? empresaId = user.EsEmpresa() ? (user.GetEmpresaId() ?? 0) : null;

                var filas = await reporteService.GetOfertasPorTipoAsync(empresaId);
                return Results.Ok(filas);
            })
            .WithName("ReporteOfertasPorTipo")
            .WithTags("Reportes")
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);
        }
    }
}
