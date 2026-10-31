// Guía de estudio: Cliente HTTP de Curriculum: encapsula llamadas a las rutas de la API y la serialización de DTO, para que las pantallas no repitan la comunicación con el servidor.
using DTOs;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Clients
{
    public class CurriculumApiClient
    {
        private readonly HttpClient _httpClient;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public CurriculumApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Consulta los metadatos del currículum del alumno sin descargar el PDF.
        public async Task<CurriculumDTO?> GetInfoAsync(int alumnoId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"alumnos/{alumnoId}/cv/info");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<CurriculumDTO>();

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener el CV");
        }

        // Recupera el PDF desde la API y solicita su guardado desde la interfaz.
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

        // Envía el PDF a la API como contenido multipart para guardar o reemplazar el currículum.
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

        // Procesa la eliminación solicitada y actualiza la información presentada por la pantalla.
        public async Task EliminarAsync(int alumnoId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"alumnos/{alumnoId}/cv");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar el CV");
        }
    }
}
