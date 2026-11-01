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
            using HttpResponseMessage response = await _httpClient.GetAsync($"ofertas/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<OfertaDTO>())!;

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new KeyNotFoundException("La oferta no existe o ya no está disponible.");

            throw await ApiErrors.CrearExcepcionAsync(response, "No se pudo cargar la oferta");
        }

        public async Task<IEnumerable<OfertaDTO>> GetAllAsync()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync("ofertas");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<OfertaDTO>>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "No se pudieron cargar las ofertas");
        }

        public async Task<IEnumerable<OfertaDTO>> GetByCriteriaAsync(OfertaCriteriaDTO criteria)
        {
            var parameters = new List<string>();
            void Add(string key, string? value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    parameters.Add(key + "=" + Uri.EscapeDataString(value));
            }
            Add("texto", criteria.Texto);
            Add("tipoOfertaId", criteria.TipoOfertaId?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            Add("estado", criteria.Estado);
            Add("desde", criteria.Desde?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
            Add("hasta", criteria.Hasta?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
            using var response = await _httpClient.GetAsync("ofertas/criteria?" + string.Join("&", parameters));
            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al buscar ofertas");
            return await response.Content.ReadFromJsonAsync<List<OfertaDTO>>() ?? new List<OfertaDTO>();
        }

        public async Task AddAsync(OfertaDTO oferta)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("ofertas", oferta);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "No se pudo crear la oferta");
        }

        public async Task UpdateAsync(OfertaDTO oferta)
        {
            using HttpResponseMessage response = await _httpClient.PutAsJsonAsync("ofertas", oferta);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "No se pudo actualizar la oferta");
        }

        public async Task DeleteAsync(int id)
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"ofertas/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "No se pudo eliminar la oferta");
        }
    }
}
