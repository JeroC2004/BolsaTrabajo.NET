using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{

    public partial class AgregarRegistroAlumnos : Migration
    {

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
