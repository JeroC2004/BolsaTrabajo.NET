using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class OfertaApiClient : BaseApiClient
    {
        public static async Task<OfertaDTO> GetAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync($"ofertas/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<OfertaDTO>())!;

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener oferta con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public static async Task<IEnumerable<OfertaDTO>> GetAllAsync()
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync("ofertas");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<OfertaDTO>>())!;

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de ofertas. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public static async Task<IEnumerable<OfertaDTO>> GetByCriteriaAsync(string texto)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.GetAsync($"ofertas/criteria?texto={Uri.EscapeDataString(texto)}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<OfertaDTO>>()) ?? new List<OfertaDTO>();

            await HandleUnauthorizedResponseAsync(response);
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al buscar ofertas. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public static async Task AddAsync(OfertaDTO oferta)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PostAsJsonAsync("ofertas", oferta);

            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear oferta. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public static async Task UpdateAsync(OfertaDTO oferta)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.PutAsJsonAsync("ofertas", oferta);

            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar oferta con Id {oferta.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public static async Task DeleteAsync(int id)
        {
            using var client = await CreateHttpClientAsync();
            HttpResponseMessage response = await client.DeleteAsync($"ofertas/{id}");

            if (!response.IsSuccessStatusCode)
            {
                await HandleUnauthorizedResponseAsync(response);
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar oferta con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}
