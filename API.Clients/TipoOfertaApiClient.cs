using DTOs;
using System.Net.Http.Json;
using System.Net;

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
            HttpResponseMessage response = await _httpClient.GetAsync("tiposoferta");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<TipoOfertaDTO>>())!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de tipos de oferta. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<TipoOfertaDTO> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"tiposoferta/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<TipoOfertaDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener el tipo de oferta");
        }

        public async Task AddAsync(TipoOfertaDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("tiposoferta", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear el tipo de oferta");
        }

        public async Task UpdateAsync(TipoOfertaDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("tiposoferta", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar el tipo de oferta");
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"tiposoferta/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar el tipo de oferta");
        }
    }
}
