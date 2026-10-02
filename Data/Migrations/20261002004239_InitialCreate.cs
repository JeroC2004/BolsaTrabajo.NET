using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomCarrera = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RazonSocial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rubro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposOferta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposOferta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alumnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomAlumno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApeAlumno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Legajo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Dni = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AnioCurso = table.Column<int>(type: "int", nullable: false),
                    CantMatAp = table.Column<int>(type: "int", nullable: false),
                    Promedio = table.Column<float>(type: "real", nullable: false),
                    CarreraId = table.Column<int>(type: "int", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alumnos_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TipoVinculo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaDesde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    TipoOfertaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ofertas_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ofertas_TiposOferta_TipoOfertaId",
                        column: x => x.TipoOfertaId,
                        principalTable: "TiposOferta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequisitosOferta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsExcluyente = table.Column<bool>(type: "bit", nullable: false),
                    OfertaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitosOferta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisitosOferta_Ofertas_OfertaId",
                        column: x => x.OfertaId,
                        principalTable: "Ofertas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Carreras",
                columns: new[] { "Id", "Departamento", "Duracion", "NomCarrera" },
                values: new object[,]
                {
                    { 1, "Ingeniería", 5, "Ingeniería en Sistemas de Información" },
                    { 2, "Ingeniería", 5, "Ingeniería Mecánica" },
                    { 3, "Ingeniería", 5, "Ingeniería Química" },
                    { 4, "Ingeniería", 5, "Ingeniería Industrial" },
                    { 5, "Ingeniería", 5, "Ingeniería Civil" }
                });

            migrationBuilder.InsertData(
                table: "Empresas",
                columns: new[] { "Id", "Descripcion", "RazonSocial", "Rubro" },
                values: new object[,]
                {
                    { 1, "Empresa de desarrollo de software", "TechCorp S.A.", "Tecnología" },
                    { 2, "Desarrollo de software a medida y consultoría IT", "DataSoft Rosario S.A.", "Tecnología" },
                    { 3, "Servicios de infraestructura cloud y ciberseguridad", "NubeCloud Argentina", "Tecnología" }
                });

            migrationBuilder.InsertData(
                table: "TiposOferta",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pasantía" },
                    { 2, "Primer empleo" },
                    { 3, "Empleo part-time" },
                    { 4, "Empleo full-time" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "PasswordHash", "Salt", "Username" },
                values: new object[] { 1, true, "admin@bolsatrabajo.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3/UhTWW1eosf/i6p2ydV5T/aANktq+KOQdvFvt7j9H4=", "RvmlmT923wpd20J6N8L3coCXmcKeclZ+2DJMBXdLQPE=", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_CarreraId",
                table: "Alumnos",
                column: "CarreraId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Email",
                table: "Alumnos",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Legajo",
                table: "Alumnos",
                column: "Legajo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carreras_NomCarrera",
                table: "Carreras",
                column: "NomCarrera",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_RazonSocial",
                table: "Empresas",
                column: "RazonSocial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_EmpresaId",
                table: "Ofertas",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_TipoOfertaId",
                table: "Ofertas",
                column: "TipoOfertaId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitosOferta_OfertaId",
                table: "RequisitosOferta",
                column: "OfertaId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposOferta_Nombre",
                table: "TiposOferta",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Username",
                table: "Usuarios",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alumnos");

            migrationBuilder.DropTable(
                name: "RequisitosOferta");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Carreras");

            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "TiposOferta");
        }
    }
}
