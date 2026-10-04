using System.Net.Http.Json;
using DTOs;

namespace BlazorApp.Client.Services;

public sealed class EmpresaApiClient
{
    private readonly HttpClient _httpClient;

    public EmpresaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<EmpresaDTO>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<EmpresaDTO>>(
            "empresas") ?? [];
    }

    public async Task<EmpresaDTO?> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<EmpresaDTO>(
            $"empresas/{id}");
    }

    public async Task CreateAsync(EmpresaDTO empresa)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync("empresas", empresa);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(EmpresaDTO empresa)
    {
        HttpResponseMessage response =
            await _httpClient.PutAsJsonAsync("empresas", empresa);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response =
            await _httpClient.DeleteAsync($"empresas/{id}");

        response.EnsureSuccessStatusCode();
    }
}