using DTOs;

namespace Application.Services
{
    public interface IPostulacionService
    {
        Task<IEnumerable<PostulacionDTO>> GetAllAsync(int? alumnoId = null, int? empresaId = null, int? ofertaId = null);
        Task<PostulacionDTO?> GetAsync(int id);
        Task<PostulacionDTO> AddAsync(PostulacionDTO dto);
        Task<bool> UpdateAsync(PostulacionDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
