using DTOs;

namespace Application.Services
{
    public interface ICurriculumService
    {
        Task<CurriculumDTO?> GetInfoAsync(int alumnoId);
        Task<CurriculumArchivoDTO?> GetArchivoAsync(int alumnoId);

        Task<CurriculumDTO> GuardarAsync(int alumnoId, string nombreArchivo, byte[] contenido);

        Task<bool> DeleteAsync(int alumnoId);
    }
}
