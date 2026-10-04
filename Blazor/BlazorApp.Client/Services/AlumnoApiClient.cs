using System.Net.Http.Json;
using DTOs;

namespace BlazorApp.Client.Services;

public sealed class AlumnoApiClient
{
    private readonly HttpClient _httpClient;

    public AlumnoApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<AlumnoDTO>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AlumnoDTO>>(
            "alumnos") ?? [];
    }

    public async Task<AlumnoDTO?> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<AlumnoDTO>(
            $"alumnos/{id}");
    }

    public async Task CreateAsync(AlumnoDTO alumno)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync("alumnos", alumno);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(AlumnoDTO alumno)
    {
        HttpResponseMessage response =
            await _httpClient.PutAsJsonAsync("alumnos", alumno);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response =
            await _httpClient.DeleteAsync($"alumnos/{id}");

        response.EnsureSuccessStatusCode();
    }
}