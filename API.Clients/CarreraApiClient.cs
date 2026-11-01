using DTOs;
using System.Net.Http.Json;

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
            using HttpResponseMessage response = await _httpClient.GetAsync("carreras");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<CarreraDTO>>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener lista de carreras");
        }

        public async Task<CarreraDTO> GetAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"carreras/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<CarreraDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener la carrera");
        }

        public async Task AddAsync(CarreraDTO dto)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("carreras", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear la carrera");
        }

        public async Task UpdateAsync(CarreraDTO dto)
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync("carreras", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar la carrera");
        }

        public async Task DeleteAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"carreras/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar la carrera");
        }
    }
}
