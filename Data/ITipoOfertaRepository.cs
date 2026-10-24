using Domain.Model;

namespace Data
{
    public interface ITipoOfertaRepository
    {
        Task<IEnumerable<TipoOferta>> GetAllAsync();
        Task<TipoOferta?> GetAsync(int id);
        Task AddAsync(TipoOferta tipoOferta);
        Task<bool> UpdateAsync(TipoOferta tipoOferta);
        Task<bool> DeleteAsync(int id);
        Task<bool> NameExistsAsync(string nombre, int? excludeId = null);
        Task<bool> HasOfertasAsync(int id);
    }
}
