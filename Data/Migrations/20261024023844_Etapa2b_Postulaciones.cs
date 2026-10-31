// Guía de estudio: Migración de base de datos (20261024023844_Etapa2b_Postulaciones): Up aplica los cambios de esquema y Down define su reversión. Conserva la evolución de las tablas junto con el código.
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Etapa2b_Postulaciones : Migration
    {
        // Aplica los cambios de esta migración sobre el esquema de la base de datos.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postulaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    OfertaId = table.Column<int>(type: "int", nullable: false),
                    FechaPostulacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postulaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Postulaciones_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Postulaciones_Ofertas_OfertaId",
                        column: x => x.OfertaId,
                        principalTable: "Ofertas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Postulaciones_AlumnoId_OfertaId",
                table: "Postulaciones",
                columns: new[] { "AlumnoId", "OfertaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Postulaciones_OfertaId",
                table: "Postulaciones",
                column: "OfertaId");
        }

        // Define cómo deshacer los cambios de esquema introducidos por esta migración.
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Postulaciones");
        }
    }
}
