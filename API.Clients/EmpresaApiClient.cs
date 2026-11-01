using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class EmpresaApiClient
    {
        private readonly HttpClient _httpClient;

        public EmpresaApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EmpresaSolicitudDTO>> GetSolicitudesAsync()
        {
            using var response = await _httpClient.GetAsync("empresas/solicitudes");
            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "No se pudieron cargar las solicitudes");
            return await response.Content.ReadFromJsonAsync<List<EmpresaSolicitudDTO>>() ?? new();
        }

        public async Task ResolverSolicitudAsync(int id, bool aprobar)
        {
            using var response = await _httpClient.PutAsync($"empresas/solicitudes/{id}/{(aprobar ? "aprobar" : "rechazar")}", null);
            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "No se pudo resolver la solicitud");
        }

        public async Task<IEnumerable<EmpresaDTO>> GetAllAsync()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync("empresas");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<EmpresaDTO>>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener lista de empresas");
        }

        public async Task<EmpresaDTO> GetAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"empresas/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<EmpresaDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener la empresa");
        }

        public async Task UpdateAsync(EmpresaDTO dto)
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync("empresas", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar la empresa");
        }

        public async Task DeleteAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"empresas/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar la empresa");
        }
    }
}
