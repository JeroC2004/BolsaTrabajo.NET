// Guía de estudio: Migración de base de datos (20261025120000_AgregarCurriculum): Up aplica los cambios de esquema y Down define su reversión. Conserva la evolución de las tablas junto con el código.
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(BolsaTrabajoContext))]
    [Migration("20261025120000_AgregarCurriculum")]
    public partial class AgregarCurriculum : Migration
    {
        // Aplica los cambios de esta migración sobre el esquema de la base de datos.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Curriculums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Contenido = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Tamanio = table.Column<int>(type: "int", nullable: false),
                    FechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Curriculums_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Curriculums_AlumnoId",
                table: "Curriculums",
                column: "AlumnoId",
                unique: true);
        }

        // Define cómo deshacer los cambios de esquema introducidos por esta migración.
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Curriculums");
        }
    }
}
