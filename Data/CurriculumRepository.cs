// Guía de estudio: Acceso a datos de Curriculum mediante Entity Framework Core: construye consultas y persiste cambios usando BolsaTrabajoContext. Separa los detalles de base de datos de los servicios.
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CurriculumRepository : ICurriculumRepository
    {
        private readonly BolsaTrabajoContext context;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public CurriculumRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        // Consulta los metadatos del currículum sin traer su contenido binario.
        public async Task<CurriculumResumen?> GetResumenAsync(int alumnoId)
        {
            return await context.Curriculums
                .AsNoTracking()
                .Where(c => c.AlumnoId == alumnoId)
                .Select(c => new CurriculumResumen(c.AlumnoId, c.NombreArchivo, c.Tamanio, c.FechaCarga))
                .FirstOrDefaultAsync();
        }

        // Recupera el currículum incluyendo los bytes del archivo PDF.
        public async Task<CurriculumVitae?> GetConContenidoAsync(int alumnoId)
        {
            return await context.Curriculums
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.AlumnoId == alumnoId);
        }

        // Reúne los datos editados y ejecuta el guardado, mostrando el resultado o los errores de la operación.
        public async Task GuardarAsync(CurriculumVitae curriculum)
        {
            var existente = await context.Curriculums.FirstOrDefaultAsync(c => c.AlumnoId == curriculum.AlumnoId);

            if (existente == null)
                context.Curriculums.Add(curriculum);
            else
                existente.Reemplazar(curriculum.NombreArchivo, curriculum.Contenido, curriculum.FechaCarga);

            await context.SaveChangesAsync();
        }

        // Elimina el registro indicado en la base de datos mediante el contexto de EF Core.
        public async Task<bool> DeleteAsync(int alumnoId)
        {
            var existente = await context.Curriculums.FirstOrDefaultAsync(c => c.AlumnoId == alumnoId);
            if (existente == null)
                return false;

            context.Curriculums.Remove(existente);
            await context.SaveChangesAsync();
            return true;
        }

        // Obtiene en conjunto los identificadores de alumnos con CV para evitar una consulta por cada fila.
        public async Task<HashSet<int>> GetAlumnoIdsConCurriculumAsync(IEnumerable<int> alumnoIds)
        {
            var ids = alumnoIds.Distinct().ToList();
            if (ids.Count == 0)
                return new HashSet<int>();

            var encontrados = await context.Curriculums
                .AsNoTracking()
                .Where(c => ids.Contains(c.AlumnoId))
                .Select(c => c.AlumnoId)
                .ToListAsync();

            return encontrados.ToHashSet();
        }
    }
}
