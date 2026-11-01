using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(BolsaTrabajoContext))]
    [Migration("20261101120000_UnificarTipoOferta")]
    public partial class UnificarTipoOferta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Respaldo histórico sin FK: debe sobrevivir a la eliminación de una oferta.
            migrationBuilder.CreateTable(
                name: "RespaldoVinculosOferta",
                columns: table => new
                {
                    OfertaId = table.Column<int>(type: "int", nullable: false),
                    TipoOfertaId = table.Column<int>(type: "int", nullable: false),
                    TipoVinculo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_RespaldoVinculosOferta", x => x.OfertaId));

            migrationBuilder.Sql("""
                INSERT INTO [RespaldoVinculosOferta] ([OfertaId], [TipoOfertaId], [TipoVinculo])
                SELECT [Id], [TipoOfertaId], [TipoVinculo] FROM [Ofertas];

                -- Usar la identidad evita colisiones con categorías creadas por el administrador.
                IF NOT EXISTS (SELECT 1 FROM [TiposOferta] WHERE [Nombre] = N'Práctica profesional')
                    INSERT INTO [TiposOferta] ([Nombre]) VALUES (N'Práctica profesional');

                -- El catálogo no incluía esta modalidad; conservarla antes de quitar el vínculo.
                UPDATE [Ofertas]
                SET [TipoOfertaId] = (SELECT [Id] FROM [TiposOferta] WHERE [Nombre] = N'Práctica profesional')
                WHERE [TipoVinculo] = N'PracticaProfesional';
                """);

            migrationBuilder.DropColumn(name: "TipoVinculo", table: "Ofertas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoVinculo", table: "Ofertas", type: "nvarchar(30)",
                maxLength: 30, nullable: false, defaultValue: "Pasantia");

            migrationBuilder.Sql("""
                -- Las ofertas nuevas no tienen respaldo: derivar su vínculo del catálogo actual.
                UPDATE o SET [TipoVinculo] = CASE t.[Nombre]
                    WHEN N'Primer empleo' THEN N'PrimerEmpleo'
                    WHEN N'Empleo part-time' THEN N'EmpleoPartTime'
                    WHEN N'Empleo full-time' THEN N'EmpleoFullTime'
                    WHEN N'Práctica profesional' THEN N'PracticaProfesional'
                    ELSE N'Pasantia' END
                FROM [Ofertas] o INNER JOIN [TiposOferta] t ON t.[Id] = o.[TipoOfertaId];

                UPDATE o SET [TipoVinculo] = r.[TipoVinculo]
                FROM [Ofertas] o INNER JOIN [RespaldoVinculosOferta] r ON r.[OfertaId] = o.[Id];

                -- Revertir la reclasificación automática solo si no hubo otra edición posterior
                -- y la categoría original todavía existe.
                UPDATE o SET [TipoOfertaId] = r.[TipoOfertaId]
                FROM [Ofertas] o
                INNER JOIN [RespaldoVinculosOferta] r ON r.[OfertaId] = o.[Id]
                INNER JOIN [TiposOferta] actual ON actual.[Id] = o.[TipoOfertaId]
                INNER JOIN [TiposOferta] anterior ON anterior.[Id] = r.[TipoOfertaId]
                WHERE r.[TipoVinculo] = N'PracticaProfesional'
                    AND actual.[Nombre] = N'Práctica profesional';
                """);

            // Mantener la categoría: puede haber sido creada antes o tener ofertas nuevas.
            migrationBuilder.DropTable(name: "RespaldoVinculosOferta");
        }
    }
}
