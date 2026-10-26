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
