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
    }
}