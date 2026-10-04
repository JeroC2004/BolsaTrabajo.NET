using System.Net.Http.Json;
using DTOs;

namespace BlazorApp.Client.Services;

public sealed class OfertaApiClient
{
    private readonly HttpClient _httpClient;

    public OfertaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<OfertaDTO>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<OfertaDTO>>(
            "ofertas") ?? [];
    }

    public async Task<OfertaDTO?> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<OfertaDTO>(
            $"ofertas/{id}");
    }

    public async Task<IReadOnlyList<OfertaDTO>> SearchAsync(
        string texto)
    {
        string query = Uri.EscapeDataString(texto);

        return await _httpClient.GetFromJsonAsync<List<OfertaDTO>>(
            $"ofertas/criteria?texto={query}") ?? [];
    }

    public async Task CreateAsync(OfertaDTO oferta)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync("ofertas", oferta);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(OfertaDTO oferta)
    {
        HttpResponseMessage response =
            await _httpClient.PutAsJsonAsync("ofertas", oferta);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response =
            await _httpClient.DeleteAsync($"ofertas/{id}");

        response.EnsureSuccessStatusCode();
    }
}