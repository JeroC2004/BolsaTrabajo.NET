using Domain.Model;

namespace Data
{
    public interface ITipoOfertaRepository
    {
        Task<IEnumerable<TipoOferta>> GetAllAsync();
    }
}
