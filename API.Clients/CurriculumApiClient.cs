using DTOs;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Clients
{
    public class CurriculumApiClient
    {
        private readonly HttpClient _httpClient;

        public CurriculumApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Devuelve null si el alumno todavía no cargó su CV.
        public async Task<CurriculumDTO?> GetInfoAsync(int alumnoId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"alumnos/{alumnoId}/cv/info");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<CurriculumDTO>();

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener el CV");
        }

        public async Task<CurriculumArchivoDTO> DescargarAsync(int alumnoId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"alumnos/{alumnoId}/cv");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al descargar el CV");

            var disposicion = response.Content.Headers.ContentDisposition;
            string? nombre = disposicion?.FileNameStar ?? disposicion?.FileName?.Trim('"');

            return new CurriculumArchivoDTO
            {
                NombreArchivo = string.IsNullOrWhiteSpace(nombre) ? "curriculum.pdf" : nombre,
                Contenido = await response.Content.ReadAsByteArrayAsync()
            };
        }

        // Alta o reemplazo del CV (multipart/form-data, campo "archivo").
        public async Task<CurriculumDTO?> SubirAsync(int alumnoId, string nombreArchivo, byte[] contenido)
        {
            var archivo = new ByteArrayContent(contenido);
            archivo.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            using var formulario = new MultipartFormDataContent();
            formulario.Add(archivo, "archivo", nombreArchivo);

            HttpResponseMessage response = await _httpClient.PutAsync($"alumnos/{alumnoId}/cv", formulario);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al cargar el CV");

            return await response.Content.ReadFromJsonAsync<CurriculumDTO>();
        }

        public async Task EliminarAsync(int alumnoId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"alumnos/{alumnoId}/cv");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar el CV");
        }
    }
}
