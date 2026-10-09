using System.Net.Http.Headers;

namespace API.Clients
{
   
    public static class ApiHttpClientFactory
    {
        private static readonly Lazy<HttpClient> _shared = new(Create);

        public static HttpClient Shared => _shared.Value;

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
