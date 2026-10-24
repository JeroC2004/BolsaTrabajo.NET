using Domain.Model;

namespace Data
{
    public interface IPostulacionRepository
    {
        Task AddAsync(Postulacion postulacion);
        Task<Postulacion?> GetAsync(int id);
        Task<IEnumerable<Postulacion>> GetAllAsync(int? alumnoId = null, int? empresaId = null, int? ofertaId = null);
        Task<bool> UpdateEstadoAsync(int id, EstadoPostulacion estado);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int alumnoId, int ofertaId);
    }
}
