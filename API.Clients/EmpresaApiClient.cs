using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class EmpresaApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<EmpresaDTO>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("empresas");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<EmpresaDTO>>())!;

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de empresas. Status: {response.StatusCode}, Detalle: {errorContent}");
        }
    }
}
