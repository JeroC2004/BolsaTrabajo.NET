using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    [DbContext(typeof(BolsaTrabajoContext))]
    [Migration("20261101140000_RegistroEmpresas")]
    public class RegistroEmpresas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
            name: "EstadoRegistro", table: "Empresas", type: "nvarchar(20)", maxLength: 20,
            nullable: false, defaultValue: "Aprobada");

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Las cuentas pendientes o rechazadas conservan Activo=false al volver al modelo anterior.
            migrationBuilder.DropColumn(name: "EstadoRegistro", table: "Empresas");
        }
    }
}
