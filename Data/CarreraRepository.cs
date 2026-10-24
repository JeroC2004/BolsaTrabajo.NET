using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CarreraRepository : ICarreraRepository
    {
        private readonly BolsaTrabajoContext context;

        public CarreraRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Carrera>> GetAllAsync()
        {
            return await context.Carreras.OrderBy(c => c.NomCarrera).ToListAsync();
        }

        public async Task<Carrera?> GetAsync(int id)
        {
            return await context.Carreras.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Carrera carrera)
        {
            context.Carreras.Add(carrera);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Carrera carrera)
        {
            var existing = await context.Carreras.FindAsync(carrera.Id);
            if (existing == null)
                return false;

            existing.SetNomCarrera(carrera.NomCarrera);
            existing.SetDepartamento(carrera.Departamento);
            existing.SetDuracion(carrera.Duracion);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await context.Carreras.FindAsync(id);
            if (existing == null)
                return false;

            context.Carreras.Remove(existing);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NameExistsAsync(string nomCarrera, int? excludeId = null)
        {
            var query = context.Carreras.Where(c => c.NomCarrera.ToLower() == nomCarrera.ToLower());
            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<bool> HasAlumnosAsync(int id)
        {
            return await context.Alumnos.AnyAsync(a => a.CarreraId == id);
        }
    }
}
