// Guía de estudio: Contrato del servicio de currículums: declara consultas de metadatos y archivo, guardado y eliminación del PDF sin exponer los detalles de persistencia.
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
