using Domain.Model;

namespace Data
{
    public interface ICurriculumRepository
    {
        // Solo metadatos: no trae el archivo desde la base.
        Task<CurriculumResumen?> GetResumenAsync(int alumnoId);

        Task<CurriculumVitae?> GetConContenidoAsync(int alumnoId);

        // Alta o reemplazo: cada alumno tiene un único CV.
        Task GuardarAsync(CurriculumVitae curriculum);

        Task<bool> DeleteAsync(int alumnoId);

        // De los alumnos indicados, devuelve los que tienen CV cargado.
        Task<HashSet<int>> GetAlumnoIdsConCurriculumAsync(IEnumerable<int> alumnoIds);
    }
}
