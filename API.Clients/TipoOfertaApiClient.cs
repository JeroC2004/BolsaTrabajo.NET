using DTOs;
using System.Net.Http.Json;

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
            using HttpResponseMessage response = await _httpClient.GetAsync("tiposoferta");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<TipoOfertaDTO>>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener lista de tipos de oferta");
        }

        public async Task<TipoOfertaDTO> GetAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"tiposoferta/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<TipoOfertaDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener el tipo de oferta");
        }

        public async Task AddAsync(TipoOfertaDTO dto)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("tiposoferta", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear el tipo de oferta");
        }

        public async Task UpdateAsync(TipoOfertaDTO dto)
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync("tiposoferta", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar el tipo de oferta");
        }

        public async Task DeleteAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"tiposoferta/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar el tipo de oferta");
        }
    }
}
