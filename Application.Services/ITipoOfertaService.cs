// Guía de estudio: Contrato del servicio de TipoOferta: declara los casos de uso que la API puede invocar, independientemente de su implementación.
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
