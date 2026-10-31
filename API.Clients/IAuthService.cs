// Guía de estudio: Contrato de autenticación para las interfaces: iniciar y cerrar sesión, obtener token y usuario y comprobar expiración. Permite implementaciones diferentes para escritorio y navegador.
namespace API.Clients
{
    public interface IAuthService
    {
        event Action<bool>? AuthenticationStateChanged;

        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetTokenAsync();
        Task<string?> GetUsernameAsync();
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task CheckTokenExpirationAsync();
    }
}
