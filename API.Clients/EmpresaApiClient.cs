using DTOs;
using System.Net.Http.Json;
using System.Net;

namespace API.Clients
{
    public class EmpresaApiClient
    {
        private readonly HttpClient _httpClient;

        public EmpresaApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<EmpresaDTO>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("empresas");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<EmpresaDTO>>())!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de empresas. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<EmpresaDTO> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"empresas/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<EmpresaDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener la empresa");
        }

        public async Task AddAsync(EmpresaDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("empresas", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear la empresa");
        }

        public async Task UpdateAsync(EmpresaDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("empresas", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar la empresa");
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"empresas/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar la empresa");
        }
    }
}
