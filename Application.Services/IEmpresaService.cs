using DTOs;

namespace Application.Services
{
    public interface IEmpresaService
    {
        Task<IEnumerable<EmpresaDTO>> GetAllAsync();
        Task<EmpresaDTO?> GetAsync(int id);
    }
}
