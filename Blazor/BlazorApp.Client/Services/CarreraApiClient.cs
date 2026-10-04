using System.Net.Http.Json;
using DTOs;

namespace BlazorApp.Client.Services;

public sealed class CarreraApiClient
{
    private readonly HttpClient _httpClient;

    public CarreraApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CarreraDTO>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CarreraDTO>>(
            "carreras") ?? [];
    }

    public async Task<CarreraDTO?> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<CarreraDTO>(
            $"carreras/{id}");
    }

    public async Task CreateAsync(CarreraDTO carrera)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync("carreras", carrera);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(CarreraDTO carrera)
    {
        HttpResponseMessage response =
            await _httpClient.PutAsJsonAsync("carreras", carrera);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response =
            await _httpClient.DeleteAsync($"carreras/{id}");

        response.EnsureSuccessStatusCode();
    }
}