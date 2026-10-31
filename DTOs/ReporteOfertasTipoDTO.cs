// Guía de estudio: Contrato de datos ReporteOfertasTipoDTO: agrupa los campos intercambiados entre API y clientes. Las propiedades describen el contenido de la solicitud o respuesta; no realizan acceso a base de datos.
namespace DTOs
{
    public class ReporteOfertasTipoDTO
    {
        public string TipoOferta { get; set; } = string.Empty;
        public int Activas { get; set; }
        public int NoActivas { get; set; }
        public int Total { get; set; }
    }
}
