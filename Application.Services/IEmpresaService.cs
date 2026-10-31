// Guía de estudio: Contrato del servicio de Empresa: declara los casos de uso que la API puede invocar, independientemente de su implementación.
using DTOs;

namespace Application.Services
{
    public interface IEmpresaService
    {
        Task<IEnumerable<EmpresaDTO>> GetAllAsync();
        Task<EmpresaDTO?> GetAsync(int id);
        Task<EmpresaDTO> AddAsync(EmpresaDTO dto);
        Task<bool> UpdateAsync(EmpresaDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
