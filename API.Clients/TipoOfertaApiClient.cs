using DTOs;
using System.Net.Http.Json;
using System.Net;

namespace API.Clients
{
    public class TipoOfertaApiClient
    {
        private readonly HttpClient _httpClient;

        public TipoOfertaApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TipoOfertaDTO>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("tiposoferta");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<TipoOfertaDTO>>())!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de tipos de oferta. Status: {response.StatusCode}, Detalle: {errorContent}");
        }
    }
}