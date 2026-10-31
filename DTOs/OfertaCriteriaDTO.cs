// Guía de estudio: DTO de filtros de OfertaCriteria: transporta criterios de búsqueda desde las interfaces hasta la API sin representar una entidad persistida.
namespace DTOs
{
    public class OfertaCriteriaDTO
    {
        public string Texto { get; set; } = string.Empty;
        public int? TipoOfertaId { get; set; }
        public string? Estado { get; set; }
        public string? TipoVinculo { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }
}
