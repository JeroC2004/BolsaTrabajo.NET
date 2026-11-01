using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CarreraDTO>> GetCarrerasRegistroAsync()
        {
            using var response = await _httpClient.GetAsync("auth/carreras");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CarreraDTO>>() ?? new List<CarreraDTO>();
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            using var response = await _httpClient.PostAsJsonAsync("auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }

            return null;
        }

        public async Task<(bool Success, string? Error)> RegisterAsync(
            RegisterRequest request)
        {
            using HttpResponseMessage response =
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

        public async Task RegisterEmpresaAsync(EmpresaRegistroRequest request)
        {
            using var response = await _httpClient.PostAsJsonAsync("auth/register/empresa", request);
            if (!response.IsSuccessStatusCode)
                throw await ApiErrors.CrearExcepcionAsync(response, "No se pudo enviar la solicitud");
        }
    }
}
