// Guía de estudio: Migración de base de datos (20261021014354_AgregarRegistroAlumnos): Up aplica los cambios de esquema y Down define su reversión. Conserva la evolución de las tablas junto con el código.
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{

    public partial class AgregarRegistroAlumnos : Migration
    {

        // Aplica los cambios de esta migración sobre el esquema de la base de datos.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "0WDIU6VcDofgFSmUIq7Y4Tt+R2xYv/OkzE1ExzsL124=", "2Bgx84tQLSMZxjgh2ePErVRnf+/XjycNJ8rd515E7j0=" });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Dni",
                table: "Alumnos",
                column: "Dni",
                unique: true);
        }

        // Define cómo deshacer los cambios de esquema introducidos por esta migración.
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alumnos_Dni",
                table: "Alumnos");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "3/UhTWW1eosf/i6p2ydV5T/aANktq+KOQdvFvt7j9H4=", "RvmlmT923wpd20J6N8L3coCXmcKeclZ+2DJMBXdLQPE=" });
        }
    }
}
