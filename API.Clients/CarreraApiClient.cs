using DTOs;
using System.Net.Http.Json;
using System.Net;

namespace API.Clients
{
    public class CarreraApiClient
    {
        private readonly HttpClient _httpClient;

        public CarreraApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CarreraDTO>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("carreras");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<CarreraDTO>>())!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de carreras. Status: {response.StatusCode}, Detalle: {errorContent}");
        }
    }
}