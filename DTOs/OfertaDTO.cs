// Guía de estudio: Contrato de datos OfertaDTO: agrupa los campos intercambiados entre API y clientes. Las propiedades describen el contenido de la solicitud o respuesta; no realizan acceso a base de datos.
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
        public List<RequisitoOfertaDTO> Requisitos { get; set; } = new(); public int EmpresaId { get; set; }
        public string? EmpresaNombre { get; set; }
        public int TipoOfertaId { get; set; }
        public string? TipoOfertaNombre { get; set; }
    }
}
