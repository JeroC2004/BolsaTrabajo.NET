using Domain.Model;

namespace Data
{
    public interface IAlumnoRepository
    {
        Task AddAsync(Alumno alumno);
        Task<bool> DeleteAsync(int id);
        Task<Alumno?> GetAsync(int id);
        Task<IEnumerable<Alumno>> GetAllAsync();
        Task<bool> UpdateAsync(Alumno alumno);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<bool> LegajoExistsAsync(string legajo, int? excludeId = null);
        Task<IEnumerable<Alumno>> GetByCriteriaAsync(AlumnoCriteria criteria);
    }
}
