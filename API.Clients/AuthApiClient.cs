// Guía de estudio: Cliente HTTP de Auth: encapsula llamadas a las rutas de la API y la serialización de DTO, para que las pantallas no repitan la comunicación con el servidor.
using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Consulta el catálogo público de carreras que necesita un alumno antes de crear su cuenta.
        public async Task<IEnumerable<CarreraDTO>> GetCarrerasRegistroAsync()
        {
            using var response = await _httpClient.GetAsync("auth/carreras");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CarreraDTO>>() ?? new List<CarreraDTO>();
        }

        // Comprueba las credenciales mediante la implementación de autenticación de esta capa y devuelve el resultado del acceso.
        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }

            return null;
        }

        public async Task<(bool Success, string? Error)> RegisterAsync(
            RegisterRequest request)
        {
            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync(
                    "auth/register",
                    request);

            if (response.IsSuccessStatusCode)
                return (true, null);

            string error = await response.Content.ReadAsStringAsync();

            return (
                false,
                string.IsNullOrWhiteSpace(error)
                    ? "No fue posible completar el registro."
                    : error);
        }
    }
}