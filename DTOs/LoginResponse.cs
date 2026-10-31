// Guía de estudio: Contrato de datos LoginResponse: agrupa los campos intercambiados entre API y clientes. Las propiedades describen el contenido de la solicitud o respuesta; no realizan acceso a base de datos.
namespace DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
