// Guía de estudio: Migración de base de datos (20261022150951_Etapa1_Roles): Up aplica los cambios de esquema y Down define su reversión. Conserva la evolución de las tablas junto con el código.
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Etapa1_Roles : Migration
    {
        // Aplica los cambios de esta migración sobre el esquema de la base de datos.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alumnos_Dni",
                table: "Alumnos");

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "AlumnoId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Usuarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_AlumnoId",
                table: "Usuarios",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmpresaId",
                table: "Usuarios",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Alumnos_AlumnoId",
                table: "Usuarios",
                column: "AlumnoId",
                principalTable: "Alumnos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Empresas_EmpresaId",
                table: "Usuarios",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        // Define cómo deshacer los cambios de esquema introducidos por esta migración.
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Alumnos_AlumnoId",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Empresas_EmpresaId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_AlumnoId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_EmpresaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "AlumnoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Usuarios");

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "PasswordHash", "Salt", "Username" },
                values: new object[] { 1, true, "admin@bolsatrabajo.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0WDIU6VcDofgFSmUIq7Y4Tt+R2xYv/OkzE1ExzsL124=", "2Bgx84tQLSMZxjgh2ePErVRnf+/XjycNJ8rd515E7j0=", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Dni",
                table: "Alumnos",
                column: "Dni",
                unique: true);
        }
    }
}
