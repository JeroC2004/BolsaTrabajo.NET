// Guía de estudio: Contrato de persistencia de Reporte: declara las operaciones disponibles sin exponer a los servicios cómo se consulta o guarda cada dato.
using Domain.Model;

namespace Data
{
   
    public class OfertaPostulacionesResumen
    {
        public int OfertaId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public EstadoOferta Estado { get; set; }
        public int Total { get; set; }
        public int Pendientes { get; set; }
        public int Aceptadas { get; set; }
        public int Rechazadas { get; set; }
    }

    public class TipoOfertaResumen
    {
        public string Nombre { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Activas { get; set; }
    }

    public interface IReporteRepository
    {
        Task<IEnumerable<OfertaPostulacionesResumen>> GetPostulacionesPorOfertaAsync(int? empresaId, DateTime? desde, DateTime? hasta);
        Task<IEnumerable<TipoOfertaResumen>> GetOfertasPorTipoAsync(int? empresaId);
    }
}
