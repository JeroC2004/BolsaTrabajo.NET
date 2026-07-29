using Domain.Model;

namespace Data
{
    public interface ICarreraRepository
    {
        Task<IEnumerable<Carrera>> GetAllAsync();
        Task<Carrera?> GetAsync(int id);
    }
}
