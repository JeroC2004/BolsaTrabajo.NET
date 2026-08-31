using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class TipoOfertaApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<TipoOfertaDTO>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("tiposoferta");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<TipoOfertaDTO>>())!;

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de tipos de oferta. Status: {response.StatusCode}, Detalle: {errorContent}");
        }
    }
}
