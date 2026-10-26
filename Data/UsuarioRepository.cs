using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByUsernameAsync(string username);
        Task RegisterAlumnoAsync(Alumno alumno, string username, string password);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BolsaTrabajoContext context;

        public UsuarioRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
        {
            return await context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Activo);
        }

        public async Task RegisterAlumnoAsync(Alumno alumno, string username, string password)
        {
            if (!await context.Carreras.AnyAsync(c => c.Id == alumno.CarreraId))
                throw new ArgumentException("La carrera seleccionada no existe.", nameof(alumno));

            if (await context.Usuarios.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
                throw new InvalidOperationException("Ese nombre de usuario ya está registrado.");

            if (await context.Usuarios.AnyAsync(u => u.Email.ToLower() == alumno.Email.ToLower()) ||
                await context.Alumnos.AnyAsync(a => a.Email.ToLower() == alumno.Email.ToLower()))
                throw new InvalidOperationException("Ya existe una cuenta registrada con ese email.");

            if (await context.Alumnos.AnyAsync(a => a.Legajo.ToLower() == alumno.Legajo.ToLower()))
                throw new InvalidOperationException("Ya existe un alumno con ese legajo.");

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                context.Alumnos.Add(alumno);
                await context.SaveChangesAsync();

                var usuario = new Usuario(
                    0,
                    username,
                    alumno.Email,
                    password,
                    DateTime.UtcNow,
                    activo: true,
                    rol: RolUsuario.Alumno,
                    alumnoId: alumno.Id);

                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.SqlClient.SqlException sql &&
                (sql.Number == 2601 || sql.Number == 2627))
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("El usuario, email o legajo ya fue registrado.", ex);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
