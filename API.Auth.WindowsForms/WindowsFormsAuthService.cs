// Guía de estudio: Implementa la sesión de escritorio: solicita el JWT a la API, mantiene los datos de acceso en memoria y comprueba su expiración.
using DTOs;
using API.Clients;

namespace API.Auth.WindowsForms
{
    public class WindowsFormsAuthService : IAuthService
    {
        private static string? _currentToken;
        private static DateTime _tokenExpiration;
        private static string? _currentUsername;

        private readonly AuthApiClient _authClient;

        public event Action<bool>? AuthenticationStateChanged;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public WindowsFormsAuthService(AuthApiClient authClient)
        {
            _authClient = authClient;
        }

        // Indica si la sesión dispone de las credenciales que esta implementación considera vigentes.
        public Task<bool> IsAuthenticatedAsync()
        {
            return Task.FromResult(!string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow < _tokenExpiration);
        }

        // Recupera el token de la sesión para autorizar las solicitudes HTTP.
        public async Task<string?> GetTokenAsync()
        {
            var isAuth = await IsAuthenticatedAsync();
            return isAuth ? _currentToken : null;
        }

        // Obtiene el nombre de usuario guardado en la sesión.
        public async Task<string?> GetUsernameAsync()
        {
            var isAuth = await IsAuthenticatedAsync();
            return isAuth ? _currentUsername : null;
        }

        // Comprueba las credenciales mediante la implementación de autenticación de esta capa y devuelve el resultado del acceso.
        public async Task<bool> LoginAsync(string username, string password)
        {
            var request = new LoginRequest { Username = username, Password = password };

            var response = await _authClient.LoginAsync(request);

            if (response != null)
            {
                _currentToken = response.Token;
                _tokenExpiration = response.ExpiresAt;
                _currentUsername = response.Username;

                AuthenticationStateChanged?.Invoke(true);
                return true;
            }

            return false;
        }

        // Limpia la sesión de esta interfaz para que deje de utilizar las credenciales guardadas.
        public Task LogoutAsync()
        {
            _currentToken = null;
            _tokenExpiration = default;
            _currentUsername = null;

            AuthenticationStateChanged?.Invoke(false);
            return Task.CompletedTask;
        }

        // Comprueba si el token sigue vigente y actualiza la sesión cuando corresponde.
        public async Task CheckTokenExpirationAsync()
        {
            if (!string.IsNullOrEmpty(_currentToken) && DateTime.UtcNow >= _tokenExpiration)
            {
                await LogoutAsync();
            }
        }
    }
}