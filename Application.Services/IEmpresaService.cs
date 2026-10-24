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
