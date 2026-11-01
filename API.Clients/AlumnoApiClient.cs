using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class AlumnoApiClient
    {
        private readonly HttpClient _httpClient;

        public AlumnoApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AlumnoDTO> GetAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"alumnos/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<AlumnoDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, $"Error al obtener alumno con Id {id}");
        }

        public async Task<IEnumerable<AlumnoDTO>> GetAllAsync()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync("alumnos");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<AlumnoDTO>>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener lista de alumnos");
        }

        public async Task<IEnumerable<AlumnoDTO>> GetByCriteriaAsync(string texto)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"alumnos/criteria?texto={Uri.EscapeDataString(texto)}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<AlumnoDTO>>()) ?? new List<AlumnoDTO>();

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al buscar alumnos");
        }

        public async Task UpdateAsync(AlumnoDTO alumno)
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync("alumnos", alumno);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar alumno");
        }

        public async Task DeleteAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"alumnos/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, $"Error al eliminar alumno con Id {id}");
        }
    }
}
