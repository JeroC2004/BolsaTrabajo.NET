using DTOs;
using System.Globalization;
using System.Net.Http.Json;

namespace API.Clients
{
    public class ReporteApiClient
    {
        private readonly HttpClient _httpClient;

        public ReporteApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ReportePostulacionesOfertaDTO>> GetPostulacionesPorOfertaAsync(
            DateTime? desde = null, DateTime? hasta = null)
        {
            var parametros = new List<string>();

            if (desde.HasValue)
                parametros.Add($"desde={desde.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

            if (hasta.HasValue)
                parametros.Add($"hasta={hasta.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

            string url = "reportes/postulaciones-por-oferta";
            if (parametros.Count > 0)
                url += "?" + string.Join("&", parametros);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<ReportePostulacionesOfertaDTO>>())
                    ?? new List<ReportePostulacionesOfertaDTO>();

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener el reporte de postulaciones");
        }

        public async Task<IEnumerable<ReporteOfertasTipoDTO>> GetOfertasPorTipoAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("reportes/ofertas-por-tipo");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<IEnumerable<ReporteOfertasTipoDTO>>())
                    ?? new List<ReporteOfertasTipoDTO>();

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener el reporte de ofertas");
        }
    }
}
