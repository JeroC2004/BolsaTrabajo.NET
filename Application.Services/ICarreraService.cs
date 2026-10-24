using DTOs;

namespace Application.Services
{
    public interface ICarreraService
    {
        Task<IEnumerable<CarreraDTO>> GetAllAsync();
        Task<CarreraDTO?> GetAsync(int id);
        Task<CarreraDTO> AddAsync(CarreraDTO dto);
        Task<bool> UpdateAsync(CarreraDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
