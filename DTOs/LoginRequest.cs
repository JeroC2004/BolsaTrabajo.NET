// Guía de estudio: Contrato de datos LoginRequest: agrupa los campos intercambiados entre API y clientes. Las propiedades describen el contenido de la solicitud o respuesta; no realizan acceso a base de datos.
namespace DTOs
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
