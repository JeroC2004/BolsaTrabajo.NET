using DTOs;

namespace Application.Services
{
    public interface ICarreraService
    {
        Task<IEnumerable<CarreraDTO>> GetAllAsync();
        Task<CarreraDTO?> GetAsync(int id);
    }
}
