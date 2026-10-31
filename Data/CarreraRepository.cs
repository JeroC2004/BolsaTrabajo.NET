// Guía de estudio: Acceso a datos de Carrera mediante Entity Framework Core: construye consultas y persiste cambios usando BolsaTrabajoContext. Separa los detalles de base de datos de los servicios.
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CarreraRepository : ICarreraRepository
    {
        private readonly BolsaTrabajoContext context;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public CarreraRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        // Obtiene la colección de registros en la base de datos mediante el contexto de EF Core.
        public async Task<IEnumerable<Carrera>> GetAllAsync()
        {
            return await context.Carreras.OrderBy(c => c.NomCarrera).ToListAsync();
        }

        // Busca un registro por su identificador en la base de datos mediante el contexto de EF Core.
        public async Task<Carrera?> GetAsync(int id)
        {
            return await context.Carreras.FirstOrDefaultAsync(c => c.Id == id);
        }

        // Agrega un registro en la base de datos mediante el contexto de EF Core.
        public async Task AddAsync(Carrera carrera)
        {
            context.Carreras.Add(carrera);
            await context.SaveChangesAsync();
        }

        // Actualiza el registro o su estado en la base de datos mediante el contexto de EF Core.
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

        // Elimina el registro indicado en la base de datos mediante el contexto de EF Core.
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

        // Comprueba relaciones existentes que pueden impedir eliminar un registro utilizado por otros datos.
        public async Task<bool> HasAlumnosAsync(int id)
        {
            return await context.Alumnos.AnyAsync(a => a.CarreraId == id);
        }
    }
}
