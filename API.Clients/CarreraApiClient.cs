// Guía de estudio: Cliente HTTP de Carrera: encapsula llamadas a las rutas de la API y la serialización de DTO, para que las pantallas no repitan la comunicación con el servidor.
using DTOs;
using System.Net.Http.Json;
using System.Net;

namespace API.Clients
{
    public class CarreraApiClient
    {
        private readonly HttpClient _httpClient;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public CarreraApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtiene la colección de registros mediante una solicitud HTTP a la API.
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

        // Busca un registro por su identificador mediante una solicitud HTTP a la API.
        public async Task<CarreraDTO> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"carreras/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<CarreraDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener la carrera");
        }

        // Agrega un registro mediante una solicitud HTTP a la API.
        public async Task AddAsync(CarreraDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("carreras", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear la carrera");
        }

        // Actualiza el registro o su estado mediante una solicitud HTTP a la API.
        public async Task UpdateAsync(CarreraDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("carreras", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar la carrera");
        }

        // Elimina el registro indicado mediante una solicitud HTTP a la API.
        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"carreras/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar la carrera");
        }
    }
}
