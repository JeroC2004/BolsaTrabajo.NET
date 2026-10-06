using DTOs;
using System.Net.Http.Json;
using System.Net;

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
            HttpResponseMessage response = await _httpClient.GetAsync($"alumnos/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<AlumnoDTO>())!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener alumno con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<IEnumerable<AlumnoDTO>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("alumnos");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<AlumnoDTO>>())!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de alumnos. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<IEnumerable<AlumnoDTO>> GetByCriteriaAsync(string texto)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"alumnos/criteria?texto={Uri.EscapeDataString(texto)}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<AlumnoDTO>>()) ?? new List<AlumnoDTO>();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al buscar alumnos. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task AddAsync(AlumnoDTO alumno)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("alumnos", alumno);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();

                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear alumno. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task UpdateAsync(AlumnoDTO alumno)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("alumnos", alumno);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();

                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar alumno con Id {alumno.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"alumnos/{id}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();

                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar alumno con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}