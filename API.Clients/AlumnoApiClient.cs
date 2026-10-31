// Guía de estudio: Cliente HTTP de Alumno: encapsula llamadas a las rutas de la API y la serialización de DTO, para que las pantallas no repitan la comunicación con el servidor.
using DTOs;
using System.Net.Http.Json;
using System.Net;

namespace API.Clients
{
    public class AlumnoApiClient
    {
        private readonly HttpClient _httpClient;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public AlumnoApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Busca un registro por su identificador mediante una solicitud HTTP a la API.
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

        // Obtiene la colección de registros mediante una solicitud HTTP a la API.
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

        // Consulta registros con los criterios de búsqueda recibidos mediante una solicitud HTTP a la API.
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

        // Agrega un registro mediante una solicitud HTTP a la API.
        public async Task AddAsync(AlumnoDTO alumno)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("alumnos", alumno);

            if (!response.IsSuccessStatusCode)
            {
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear alumno");
            }
        }

        // Actualiza el registro o su estado mediante una solicitud HTTP a la API.
        public async Task UpdateAsync(AlumnoDTO alumno)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("alumnos", alumno);

            if (!response.IsSuccessStatusCode)
            {
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar alumno");
            }
        }

        // Elimina el registro indicado mediante una solicitud HTTP a la API.
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
