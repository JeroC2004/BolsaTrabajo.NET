// Guía de estudio: Contrato de persistencia de TipoOferta: declara las operaciones disponibles sin exponer a los servicios cómo se consulta o guarda cada dato.
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
