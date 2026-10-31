// Guía de estudio: Contrato del servicio de Alumno: declara los casos de uso que la API puede invocar, independientemente de su implementación.
using DTOs;

namespace Application.Services
{
    public interface IAlumnoService
    {
        Task<AlumnoDTO> AddAsync(AlumnoDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<AlumnoDTO?> GetAsync(int id);
        Task<IEnumerable<AlumnoDTO>> GetAllAsync();
        Task<bool> UpdateAsync(AlumnoDTO dto);
        Task<IEnumerable<AlumnoDTO>> GetByCriteriaAsync(AlumnoCriteriaDTO criteriaDTO);
    }
}
