using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class CarreraApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<CarreraDTO>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("carreras");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<CarreraDTO>>())!;

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de carreras. Status: {response.StatusCode}, Detalle: {errorContent}");
        }
    }
}
