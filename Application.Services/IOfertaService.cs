using DTOs;

namespace Application.Services
{
    public interface IOfertaService
    {
        Task<OfertaDTO> AddAsync(OfertaDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<OfertaDTO?> GetAsync(int id);
        Task<IEnumerable<OfertaDTO>> GetAllAsync();
        Task<bool> UpdateAsync(OfertaDTO dto);
        Task<IEnumerable<OfertaDTO>> GetByCriteriaAsync(OfertaCriteriaDTO criteriaDTO);
    }
}
