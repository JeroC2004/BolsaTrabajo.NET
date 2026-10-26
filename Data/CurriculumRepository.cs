using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CurriculumRepository : ICurriculumRepository
    {
        private readonly BolsaTrabajoContext context;

        public CurriculumRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task<CurriculumResumen?> GetResumenAsync(int alumnoId)
        {
            return await context.Curriculums
                .AsNoTracking()
                .Where(c => c.AlumnoId == alumnoId)
                .Select(c => new CurriculumResumen(c.AlumnoId, c.NombreArchivo, c.Tamanio, c.FechaCarga))
                .FirstOrDefaultAsync();
        }

        public async Task<CurriculumVitae?> GetConContenidoAsync(int alumnoId)
        {
            return await context.Curriculums
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.AlumnoId == alumnoId);
        }

        public async Task GuardarAsync(CurriculumVitae curriculum)
        {
            var existente = await context.Curriculums.FirstOrDefaultAsync(c => c.AlumnoId == curriculum.AlumnoId);

            if (existente == null)
                context.Curriculums.Add(curriculum);
            else
                existente.Reemplazar(curriculum.NombreArchivo, curriculum.Contenido, curriculum.FechaCarga);

            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int alumnoId)
        {
            var existente = await context.Curriculums.FirstOrDefaultAsync(c => c.AlumnoId == alumnoId);
            if (existente == null)
                return false;

            context.Curriculums.Remove(existente);
            await context.SaveChangesAsync();
            return true;
        }

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
