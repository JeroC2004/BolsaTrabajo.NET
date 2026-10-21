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