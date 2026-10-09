using System.Net;
using System.Net.Http.Headers;

namespace API.Clients
{
    public abstract class BaseApiClient
    {
        protected static async Task<HttpClient> CreateHttpClientAsync()
        {
            var client = new HttpClient();
            await ConfigureHttpClientAsync(client);
            return client;
        }

        protected static async Task ConfigureHttpClientAsync(HttpClient client)
        {
            string baseUrl = GetBaseUrlFromConfig();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            await AddAuthorizationHeaderAsync(client);
        }

        private static string GetBaseUrlFromConfig() => ApiConfiguration.BaseUrl;

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
