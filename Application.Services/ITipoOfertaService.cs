using DTOs;

namespace Application.Services
{
    public interface ITipoOfertaService
    {
        Task<IEnumerable<TipoOfertaDTO>> GetAllAsync();
        Task<TipoOfertaDTO?> GetAsync(int id);
        Task<TipoOfertaDTO> AddAsync(TipoOfertaDTO dto);
        Task<bool> UpdateAsync(TipoOfertaDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
