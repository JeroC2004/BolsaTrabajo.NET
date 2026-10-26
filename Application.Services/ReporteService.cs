using Data;
using DTOs;

namespace Application.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository reporteRepository;

        public ReporteService(IReporteRepository reporteRepository)
        {
            this.reporteRepository = reporteRepository;
        }

        public async Task<IEnumerable<ReportePostulacionesOfertaDTO>> GetPostulacionesPorOfertaAsync(
            int? empresaId, DateTime? desde, DateTime? hasta)
        {
            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
                throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta.");

            var filas = await reporteRepository.GetPostulacionesPorOfertaAsync(empresaId, desde, hasta);

            return filas
                .OrderByDescending(f => f.Total)
                .ThenBy(f => f.Titulo)
                .Select(f => new ReportePostulacionesOfertaDTO
                {
                    OfertaId = f.OfertaId,
                    Titulo = f.Titulo,
                    Empresa = f.Empresa,
                    EstadoOferta = f.Estado.ToString(),
                    Total = f.Total,
                    Pendientes = f.Pendientes,
                    Aceptadas = f.Aceptadas,
                    Rechazadas = f.Rechazadas
                })
                .ToList();
        }

        public async Task<IEnumerable<ReporteOfertasTipoDTO>> GetOfertasPorTipoAsync(int? empresaId)
        {
            var filas = await reporteRepository.GetOfertasPorTipoAsync(empresaId);

            return filas
                .Select(f => new ReporteOfertasTipoDTO
                {
                    TipoOferta = f.Nombre,
                    Activas = f.Activas,
                    NoActivas = f.Total - f.Activas,
                    Total = f.Total
                })
                .ToList();
        }
    }
}
