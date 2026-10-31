// Guía de estudio: Contrato del servicio de Reporte: declara los casos de uso que la API puede invocar, independientemente de su implementación.
using DTOs;

namespace Application.Services
{
    public interface IReporteService
    {
        Task<IEnumerable<ReportePostulacionesOfertaDTO>> GetPostulacionesPorOfertaAsync(int? empresaId, DateTime? desde, DateTime? hasta);
        Task<IEnumerable<ReporteOfertasTipoDTO>> GetOfertasPorTipoAsync(int? empresaId);
    }
}
