// Guía de estudio: Contrato de persistencia de Oferta: declara las operaciones disponibles sin exponer a los servicios cómo se consulta o guarda cada dato.
using Domain.Model;

namespace Data
{
    public interface IOfertaRepository
    {
        Task AddAsync(Oferta oferta);
        Task<bool> DeleteAsync(int id);
        Task<Oferta?> GetAsync(int id);
        Task<IEnumerable<Oferta>> GetAllAsync();
        Task<bool> UpdateAsync(Oferta oferta);
        Task<IEnumerable<Oferta>> GetByCriteriaAsync(OfertaCriteria criteria);
    }
}
