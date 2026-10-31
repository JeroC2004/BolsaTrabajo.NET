// Guía de estudio: Cliente HTTP de Postulacion: encapsula llamadas a las rutas de la API y la serialización de DTO, para que las pantallas no repitan la comunicación con el servidor.
using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class PostulacionApiClient
    {
        private readonly HttpClient _httpClient;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public PostulacionApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Consulta la colección de registros, respetando los filtros opcionales recibidos.
        public async Task<IEnumerable<PostulacionDTO>> GetAllAsync(int? ofertaId = null)
        {
            string url = ofertaId.HasValue ? $"postulaciones?ofertaId={ofertaId.Value}" : "postulaciones";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<PostulacionDTO>>()) ?? new List<PostulacionDTO>();

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener las postulaciones");
        }

        // Busca un registro por su identificador mediante una solicitud HTTP a la API.
        public async Task<PostulacionDTO> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"postulaciones/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<PostulacionDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener la postulación");
        }

        // Agrega un registro mediante una solicitud HTTP a la API.
        public async Task AddAsync(PostulacionDTO postulacion)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("postulaciones", postulacion);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear la postulación");
        }

        // Actualiza el registro o su estado mediante una solicitud HTTP a la API.
        public async Task UpdateAsync(PostulacionDTO postulacion)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("postulaciones", postulacion);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar la postulación");
        }

        // Elimina el registro indicado mediante una solicitud HTTP a la API.
        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"postulaciones/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar la postulación");
        }
    }
}
