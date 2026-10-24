using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
  
    public static class DbSeeder
    {
        private const string PasswordEmpresa = "empresa123";
        private const string PasswordAlumno = "alumno123";

        public static async Task SeedAsync(BolsaTrabajoContext context)
        {
            
            await context.Database.ExecuteSqlRawAsync(
                "UPDATE Usuarios SET Rol = 'Administrador' WHERE Rol IS NULL OR Rol = ''");

            await AsegurarUsuarioAsync(context, "admin", "admin@bolsatrabajo.com", "admin123", RolUsuario.Administrador);

            // Las empresas (Ids 1 a 3) se cargan por migración.
            await AsegurarUsuarioEmpresaAsync(context, 1, "techcorp", "rrhh@techcorp.com");
            await AsegurarUsuarioEmpresaAsync(context, 2, "datasoft", "rrhh@datasoft.com");
            await AsegurarUsuarioEmpresaAsync(context, 3, "nubecloud", "rrhh@nubecloud.com");

            await AsegurarAlumnoConUsuarioAsync(context, "52001", "Lucía", "Fernández",
                "lucia.fernandez@alumnos.utn.edu.ar", "40123456", "2023", 4, 28, 8.2f, 1, "lucia");
            await AsegurarAlumnoConUsuarioAsync(context, "52002", "Mateo", "Gómez",
                "mateo.gomez@alumnos.utn.edu.ar", "41234567", "2023", 3, 20, 7.5f, 1, "mateo");
        }

        private static async Task AsegurarUsuarioEmpresaAsync(BolsaTrabajoContext context, int empresaId, string username, string email)
        {
            if (!await context.Empresas.AnyAsync(e => e.Id == empresaId))
                return;

            await AsegurarUsuarioAsync(context, username, email, PasswordEmpresa, RolUsuario.Empresa, empresaId: empresaId);
        }

        private static async Task AsegurarAlumnoConUsuarioAsync(BolsaTrabajoContext context, string legajo, string nombre,
            string apellido, string email, string dni, string plan, int anioCurso, int materiasAprobadas,
            float promedio, int carreraId, string username)
        {
            if (!await context.Carreras.AnyAsync(c => c.Id == carreraId))
                return;

            var alumno = await context.Alumnos.FirstOrDefaultAsync(a => a.Legajo == legajo);
            if (alumno == null)
            {
                alumno = new Alumno(0, nombre, apellido, email, legajo, dni, plan, anioCurso,
                    materiasAprobadas, promedio, carreraId, DateTime.Today);
                context.Alumnos.Add(alumno);
                await context.SaveChangesAsync();
            }

            await AsegurarUsuarioAsync(context, username, email, PasswordAlumno, RolUsuario.Alumno, alumnoId: alumno.Id);
        }

        private static async Task AsegurarUsuarioAsync(BolsaTrabajoContext context, string username, string email,
            string password, RolUsuario rol, int? alumnoId = null, int? empresaId = null)
        {
            var existente = await context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

            if (existente == null)
            {
                context.Usuarios.Add(new Usuario(0, username, email, password, DateTime.UtcNow, true, rol, alumnoId, empresaId));
                await context.SaveChangesAsync();
                return;
            }

            if (existente.Rol != rol || existente.AlumnoId != alumnoId || existente.EmpresaId != empresaId || !existente.Activo)
            {
                existente.SetRol(rol, alumnoId, empresaId);
                existente.SetActivo(true);
                await context.SaveChangesAsync();
            }
        }
    }
}
