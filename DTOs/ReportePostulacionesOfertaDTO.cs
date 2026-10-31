// Guía de estudio: Contrato de datos ReportePostulacionesOfertaDTO: agrupa los campos intercambiados entre API y clientes. Las propiedades describen el contenido de la solicitud o respuesta; no realizan acceso a base de datos.
namespace DTOs
{
    public class ReportePostulacionesOfertaDTO
    {
        public int OfertaId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string EstadoOferta { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Pendientes { get; set; }
        public int Aceptadas { get; set; }
        public int Rechazadas { get; set; }
    }
}
