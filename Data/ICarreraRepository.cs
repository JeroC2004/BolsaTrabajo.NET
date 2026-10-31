// Guía de estudio: Contrato de persistencia de Carrera: declara las operaciones disponibles sin exponer a los servicios cómo se consulta o guarda cada dato.
using Domain.Model;

namespace Data
{
    public interface ICarreraRepository
    {
        Task<IEnumerable<Carrera>> GetAllAsync();
        Task<Carrera?> GetAsync(int id);
        Task AddAsync(Carrera carrera);
        Task<bool> UpdateAsync(Carrera carrera);
        Task<bool> DeleteAsync(int id);
        Task<bool> NameExistsAsync(string nomCarrera, int? excludeId = null);
        Task<bool> HasAlumnosAsync(int id);
    }
}
