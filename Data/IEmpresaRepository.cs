using Domain.Model;

namespace Data
{
    public interface IEmpresaRepository
    {
        Task<IEnumerable<Empresa>> GetAllAsync();
        Task<Empresa?> GetAsync(int id);
    }
}
