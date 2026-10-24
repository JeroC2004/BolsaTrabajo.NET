using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly BolsaTrabajoContext context;

        public ReporteRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        // Una fila por oferta (aunque no tenga postulaciones) con el total y el detalle por estado.
        public async Task<IEnumerable<OfertaPostulacionesResumen>> GetPostulacionesPorOfertaAsync(
            int? empresaId, DateTime? desde, DateTime? hasta)
        {
            IQueryable<Postulacion> postulaciones = context.Postulaciones;

            if (desde.HasValue)
            {
                DateTime fechaDesde = desde.Value.Date;
                postulaciones = postulaciones.Where(p => p.FechaPostulacion >= fechaDesde);
            }

            if (hasta.HasValue)
            {
                DateTime fechaHasta = hasta.Value.Date;
                postulaciones = postulaciones.Where(p => p.FechaPostulacion <= fechaHasta);
            }

            IQueryable<Oferta> ofertas = context.Ofertas;

            if (empresaId.HasValue)
                ofertas = ofertas.Where(o => o.EmpresaId == empresaId.Value);

            return await ofertas
                .Select(o => new OfertaPostulacionesResumen
                {
                    OfertaId = o.Id,
                    Titulo = o.Titulo,
                    Empresa = o.Empresa!.RazonSocial,
                    Estado = o.Estado,
                    Total = postulaciones.Count(p => p.OfertaId == o.Id),
                    Pendientes = postulaciones.Count(p => p.OfertaId == o.Id && p.Estado == EstadoPostulacion.Pendiente),
                    Aceptadas = postulaciones.Count(p => p.OfertaId == o.Id && p.Estado == EstadoPostulacion.Aceptada),
                    Rechazadas = postulaciones.Count(p => p.OfertaId == o.Id && p.Estado == EstadoPostulacion.Rechazada)
                })
                .ToListAsync();
        }

        // Una fila por tipo de oferta con el total de ofertas y cuántas están activas.
        public async Task<IEnumerable<TipoOfertaResumen>> GetOfertasPorTipoAsync(int? empresaId)
        {
            IQueryable<Oferta> ofertas = context.Ofertas;

            if (empresaId.HasValue)
                ofertas = ofertas.Where(o => o.EmpresaId == empresaId.Value);

            return await context.TiposOferta
                .OrderBy(t => t.Nombre)
                .Select(t => new TipoOfertaResumen
                {
                    Nombre = t.Nombre,
                    Total = ofertas.Count(o => o.TipoOfertaId == t.Id),
                    Activas = ofertas.Count(o => o.TipoOfertaId == t.Id && o.Estado == EstadoOferta.Activa)
                })
                .ToListAsync();
        }
    }
}
