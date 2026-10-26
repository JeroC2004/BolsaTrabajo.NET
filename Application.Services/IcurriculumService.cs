using DTOs;

namespace Application.Services
{
    public interface ICurriculumService
    {
        Task<CurriculumDTO?> GetInfoAsync(int alumnoId);
        Task<CurriculumArchivoDTO?> GetArchivoAsync(int alumnoId);

        // Alta o reemplazo del CV. Lanza ArgumentException si el archivo no es válido.
        Task<CurriculumDTO> GuardarAsync(int alumnoId, string nombreArchivo, byte[] contenido);

        Task<bool> DeleteAsync(int alumnoId);
    }
}
