// Guía de estudio: Contrato de persistencia de Curriculum: declara las operaciones disponibles sin exponer a los servicios cómo se consulta o guarda cada dato.
using Domain.Model;

namespace Data
{
    public interface ICurriculumRepository
    {
        Task<CurriculumResumen?> GetResumenAsync(int alumnoId);

        Task<CurriculumVitae?> GetConContenidoAsync(int alumnoId);

        Task GuardarAsync(CurriculumVitae curriculum);

        Task<bool> DeleteAsync(int alumnoId);

        Task<HashSet<int>> GetAlumnoIdsConCurriculumAsync(IEnumerable<int> alumnoIds);
    }
}
