using DTOs;

namespace Application.Services
{
    public interface ITipoOfertaService
    {
        Task<IEnumerable<TipoOfertaDTO>> GetAllAsync();
    }
}
