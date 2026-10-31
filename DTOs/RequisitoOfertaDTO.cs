// Guía de estudio: Contrato de datos RequisitoOfertaDTO: agrupa los campos intercambiados entre API y clientes. Las propiedades describen el contenido de la solicitud o respuesta; no realizan acceso a base de datos.
namespace DTOs
{
    public class RequisitoOfertaDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool EsExcluyente { get; set; }
    }
}