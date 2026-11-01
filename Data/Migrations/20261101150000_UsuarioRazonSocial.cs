using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    [DbContext(typeof(BolsaTrabajoContext))]
    [Migration("20261101150000_UsuarioRazonSocial")]
    public class UsuarioRazonSocial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<string>(
            name: "Username", table: "Usuarios", type: "nvarchar(150)", maxLength: 150, nullable: false,
            oldClrType: typeof(string), oldType: "nvarchar(50)", oldMaxLength: 50);

        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<string>(
            name: "Username", table: "Usuarios", type: "nvarchar(50)", maxLength: 50, nullable: false,
            oldClrType: typeof(string), oldType: "nvarchar(150)", oldMaxLength: 150);
    }
}
