// Guía de estudio: Cliente HTTP de Empresa: encapsula llamadas a las rutas de la API y la serialización de DTO, para que las pantallas no repitan la comunicación con el servidor.
using DTOs;
using System.Net.Http.Json;
using System.Net;

namespace API.Clients
{
    public class EmpresaApiClient
    {
        private readonly HttpClient _httpClient;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public EmpresaApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtiene la colección de registros mediante una solicitud HTTP a la API.
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

        // Busca un registro por su identificador mediante una solicitud HTTP a la API.
        public async Task<EmpresaDTO> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"empresas/{id}");

            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<EmpresaDTO>())!;

            throw await ApiErrors.CrearExcepcionAsync(response, "Error al obtener la empresa");
        }

        // Agrega un registro mediante una solicitud HTTP a la API.
        public async Task AddAsync(EmpresaDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("empresas", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al crear la empresa");
        }

        // Actualiza el registro o su estado mediante una solicitud HTTP a la API.
        public async Task UpdateAsync(EmpresaDTO dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("empresas", dto);

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al actualizar la empresa");
        }

        // Elimina el registro indicado mediante una solicitud HTTP a la API.
        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"empresas/{id}");

            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "Error al eliminar la empresa");
        }
    }
}
