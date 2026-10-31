// Guía de estudio: Crea clientes HTTP con la dirección de la API y el manejador de autorización; ofrece una instancia compartida para escritorio.
using System.Net.Http.Headers;

namespace API.Clients
{
   
    public static class ApiHttpClientFactory
    {
        private static readonly Lazy<HttpClient> _shared = new(Create);

        public static HttpClient Shared => _shared.Value;

        // Crea un HttpClient con dirección base, formato JSON y manejador de autorización.
        public static HttpClient Create()
        {
            var client = new HttpClient(new AuthorizationMessageHandler())
            {
                BaseAddress = new Uri(ApiConfiguration.BaseUrl)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
