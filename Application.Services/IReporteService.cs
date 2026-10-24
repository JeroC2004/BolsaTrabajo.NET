using DTOs;

namespace Application.Services
{
    public interface IReporteService
    {
        Task<IEnumerable<ReportePostulacionesOfertaDTO>> GetPostulacionesPorOfertaAsync(int? empresaId, DateTime? desde, DateTime? hasta);
        Task<IEnumerable<ReporteOfertasTipoDTO>> GetOfertasPorTipoAsync(int? empresaId);
    }
}
