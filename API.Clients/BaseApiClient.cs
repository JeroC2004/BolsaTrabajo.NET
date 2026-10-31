// Guía de estudio: Reúne utilidades para configurar HttpClient, agregar autorización y procesar respuestas sin autenticación.
using System.Net;
using System.Net.Http.Headers;

namespace API.Clients
{
    public abstract class BaseApiClient
    {
        // Crea un cliente HTTP y aplica la configuración común y las credenciales disponibles.
        protected static async Task<HttpClient> CreateHttpClientAsync()
        {
            var client = new HttpClient();
            await ConfigureHttpClientAsync(client);
            return client;
        }

        // Configura dirección base, formato JSON y cabecera de autorización del cliente HTTP.
        protected static async Task ConfigureHttpClientAsync(HttpClient client)
        {
            string baseUrl = GetBaseUrlFromConfig();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            await AddAuthorizationHeaderAsync(client);
        }

        // Obtiene la dirección de la API desde la configuración compartida.
        private static string GetBaseUrlFromConfig() => ApiConfiguration.BaseUrl;

        // Comprueba la expiración de sesión y agrega el token disponible como credencial Bearer.
        protected static async Task AddAuthorizationHeaderAsync(HttpClient client)
        {
            var authService = AuthServiceProvider.Instance;

            await authService.CheckTokenExpirationAsync();

            var token = await authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // Ante una respuesta 401, cierra la sesión y genera una excepción para solicitar un nuevo ingreso.
        protected static async Task HandleUnauthorizedResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var authService = AuthServiceProvider.Instance;
                await authService.LogoutAsync();
                throw new UnauthorizedAccessException("Su sesión ha expirado. Debe iniciar sesión nuevamente.");
            }
        }
    }
}
