using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Data
{
    public class BolsaTrabajoContext : DbContext
    {
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<TipoOferta> TiposOferta { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public BolsaTrabajoContext(DbContextOptions<BolsaTrabajoContext> options) : base(options)
        {
            // La base de datos se autogenera si no existe (pedido de la consigna).
            // EnsureCreated no soporta migraciones incrementales, pero para este TP
            // (sin necesidad de versionar el esquema) es la opción más simple y directa.
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Carrera>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.NomCarrera).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Departamento).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Duracion).IsRequired();
                entity.HasIndex(e => e.NomCarrera).IsUnique();

                entity.HasData(
                    new { Id = 1, NomCarrera = "Ingeniería en Sistemas de Información", Departamento = "Ingeniería", Duracion = 5 },
                    new { Id = 2, NomCarrera = "Licenciatura en Administración", Departamento = "Ciencias Económicas", Duracion = 4 },
                    new { Id = 3, NomCarrera = "Contador Público", Departamento = "Ciencias Económicas", Duracion = 5 },
                    new { Id = 4, NomCarrera = "Ingeniería Industrial", Departamento = "Ingeniería", Duracion = 5 }
                );
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.RazonSocial).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Rubro).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.RazonSocial).IsUnique();

                entity.HasData(
                    new { Id = 1, RazonSocial = "TechCorp S.A.", Descripcion = "Empresa de desarrollo de software", Rubro = "Tecnología" },
                    new { Id = 2, RazonSocial = "Banco Litoral", Descripcion = "Entidad financiera regional", Rubro = "Finanzas" },
                    new { Id = 3, RazonSocial = "Agro Insumos S.R.L.", Descripcion = "Distribuidora de insumos agropecuarios", Rubro = "Agroindustria" }
                );
            });

            modelBuilder.Entity<TipoOferta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Nombre).IsUnique();

                entity.HasData(
                    new { Id = 1, Nombre = "Pasantía" },
                    new { Id = 2, Nombre = "Primer empleo" },
                    new { Id = 3, Nombre = "Práctica profesional supervisada" },
                    new { Id = 4, Nombre = "Empleo full-time" }
                );
            });

            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.NomAlumno).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ApeAlumno).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Legajo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Dni).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Plan).IsRequired().HasMaxLength(20);
                entity.Property(e => e.AnioCurso).IsRequired();
                entity.Property(e => e.CantMatAp).IsRequired();
                entity.Property(e => e.Promedio).IsRequired();
                entity.Property(e => e.FechaAlta).IsRequired();

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Legajo).IsUnique();

                // Backing fields: el dominio expone CarreraId/Carrera con setters privados,
                // por eso hay que decirle a EF en qué campo privado persistir cada uno.
                entity.Property(e => e.CarreraId).IsRequired().HasField("_carreraId");
                entity.Navigation(e => e.Carrera).HasField("_carrera");

                entity.HasOne(e => e.Carrera)
                    .WithMany()
                    .HasForeignKey(e => e.CarreraId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Oferta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(150);
                entity.Property(e => e.TipoVinculo).IsRequired().HasConversion<string>().HasMaxLength(30);
                entity.Property(e => e.FechaDesde).IsRequired();
                entity.Property(e => e.FechaHasta).IsRequired();
                entity.Property(e => e.Detalle).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Requisitos).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Estado).IsRequired().HasConversion<string>().HasMaxLength(20);

                entity.Property(e => e.EmpresaId).IsRequired().HasField("_empresaId");
                entity.Navigation(e => e.Empresa).HasField("_empresa");
                entity.HasOne(e => e.Empresa)
                    .WithMany()
                    .HasForeignKey(e => e.EmpresaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TipoOfertaId).IsRequired().HasField("_tipoOfertaId");
                entity.Navigation(e => e.TipoOferta).HasField("_tipoOferta");
                entity.HasOne(e => e.TipoOferta)
                    .WithMany()
                    .HasForeignKey(e => e.TipoOfertaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Salt).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FechaCreacion).IsRequired();
                entity.Property(e => e.Activo).IsRequired();

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                // Usuario admin sembrado para poder loguearse la primera vez sin tener
                // que insertar nada manualmente. Password: admin123
                var admin = new Usuario(1, "admin", "admin@bolsatrabajo.com", "admin123", new DateTime(2026, 1, 1));
                entity.HasData(new
                {
                    Id = admin.Id,
                    Username = admin.Username,
                    Email = admin.Email,
                    PasswordHash = admin.PasswordHash,
                    Salt = admin.Salt,
                    FechaCreacion = admin.FechaCreacion,
                    Activo = admin.Activo
                });
            });
        }
    }
}
