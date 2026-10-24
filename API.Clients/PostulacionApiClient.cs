using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class PostulacionApiClient
    {
        private readonly HttpClient _httpClient;

        public PostulacionApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<PostulacionDTO>> GetAllAsync(int? ofertaId = null)
        {
            string url = ofertaId.HasValue ? $"postulaciones?ofertaId={ofertaId.Value}" : "postulaciones";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<PostulacionDTO>>()) ?? new List<PostulacionDTO>();

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener las postulaciones");
        }

        public async Task<PostulacionDTO> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"postulaciones/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<PostulacionDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener la postulación");
        }

        public async Task AddAsync(PostulacionDTO postulacion)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("postulaciones", postulacion);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear la postulación");
        }

        // Solo se modifica el estado (Pendiente, Aceptada o Rechazada).
        public async Task UpdateAsync(PostulacionDTO postulacion)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("postulaciones", postulacion);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar la postulación");
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"postulaciones/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar la postulación");
        }
    }
}
