// Guía de estudio: Contrato de persistencia de Empresa: declara las operaciones disponibles sin exponer a los servicios cómo se consulta o guarda cada dato.
using Domain.Model;

namespace Data
{
    public interface IEmpresaRepository
    {
        Task<IEnumerable<Empresa>> GetAllAsync();
        Task<Empresa?> GetAsync(int id);
        Task AddAsync(Empresa empresa);
        Task<bool> UpdateAsync(Empresa empresa);
        Task<bool> DeleteAsync(int id);
        Task<bool> NameExistsAsync(string razonSocial, int? excludeId = null);
        Task<bool> HasOfertasAsync(int id);
    }
}
