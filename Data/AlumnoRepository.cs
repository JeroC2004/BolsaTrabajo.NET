using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AlumnoRepository : IAlumnoRepository
    {
        private readonly BolsaTrabajoContext context;

        public AlumnoRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Alumno alumno)
        {
            context.Alumnos.Add(alumno);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var alumno = await context.Alumnos.FindAsync(id);
            if (alumno != null)
            {
                context.Alumnos.Remove(alumno);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Alumno?> GetAsync(int id)
        {
            return await context.Alumnos
                .Include(a => a.Carrera)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Alumno>> GetAllAsync()
        {
            return await context.Alumnos
                .Include(a => a.Carrera)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Alumno alumno)
        {
            var existing = await context.Alumnos.FindAsync(alumno.Id);
            if (existing != null)
            {
                existing.SetNomAlumno(alumno.NomAlumno);
                existing.SetApeAlumno(alumno.ApeAlumno);
                existing.SetEmail(alumno.Email);
                existing.SetLegajo(alumno.Legajo);
                existing.SetDni(alumno.Dni);
                existing.SetPlan(alumno.Plan);
                existing.SetAnioCurso(alumno.AnioCurso);
                existing.SetCantMatAp(alumno.CantMatAp);
                existing.SetPromedio(alumno.Promedio);
                existing.SetCarreraId(alumno.CarreraId);

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = context.Alumnos.Where(a => a.Email.ToLower() == email.ToLower());
            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<bool> LegajoExistsAsync(string legajo, int? excludeId = null)
        {
            var query = context.Alumnos.Where(a => a.Legajo.ToLower() == legajo.ToLower());
            if (excludeId.HasValue)
                query = query.Where(a => a.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria)
        {
            string searchTerm = criteria.Texto.ToLower();

            return await context.Alumnos
                .Include(a => a.Carrera)
                .Where(a =>
                    a.NomAlumno.ToLower().Contains(searchTerm) ||
                    a.ApeAlumno.ToLower().Contains(searchTerm) ||
                    a.Legajo.ToLower().Contains(searchTerm) ||
                    a.Email.ToLower().Contains(searchTerm))
                .OrderBy(a => a.ApeAlumno)
                .ThenBy(a => a.NomAlumno)
                .ToListAsync();
        }
    }
}
