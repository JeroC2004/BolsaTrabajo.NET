namespace DTOs
{
    public class OfertaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;

       
        public string TipoVinculo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Detalle { get; set; } = string.Empty;
        public string Requisitos { get; set; } = string.Empty;
        public int EmpresaId { get; set; }
        public string? EmpresaNombre { get; set; }
        public int TipoOfertaId { get; set; }
        public string? TipoOfertaNombre { get; set; }
    }
}
