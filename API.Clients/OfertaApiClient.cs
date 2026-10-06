using DTOs;
using System.Net.Http.Json;
using System.Net;

namespace API.Clients
{
    public class OfertaApiClient
    {
        private readonly HttpClient _httpClient;
        public OfertaApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OfertaDTO> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"ofertas/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<OfertaDTO>())!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener oferta con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<IEnumerable<OfertaDTO>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("ofertas");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<OfertaDTO>>())!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener lista de ofertas. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task<IEnumerable<OfertaDTO>> GetByCriteriaAsync(string texto)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"ofertas/criteria?texto={Uri.EscapeDataString(texto)}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<OfertaDTO>>()) ?? new List<OfertaDTO>();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al buscar ofertas. Status: {response.StatusCode}, Detalle: {errorContent}");
        }

        public async Task AddAsync(OfertaDTO oferta)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("ofertas", oferta);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();

                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear oferta. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task UpdateAsync(OfertaDTO oferta)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("ofertas", oferta);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();

                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar oferta con Id {oferta.Id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"ofertas/{id}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();

                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al eliminar oferta con Id {id}. Status: {response.StatusCode}, Detalle: {errorContent}");
            }
        }
    }
}