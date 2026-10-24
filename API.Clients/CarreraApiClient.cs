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

        public async Task<CarreraDTO> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"carreras/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<CarreraDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener la carrera");
        }

        public async Task AddAsync(CarreraDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("carreras", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear la carrera");
        }

        public async Task UpdateAsync(CarreraDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("carreras", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar la carrera");
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"carreras/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar la carrera");
        }
    }
}
