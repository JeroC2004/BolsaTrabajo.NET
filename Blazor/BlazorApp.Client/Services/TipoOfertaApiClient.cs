using System.Net.Http.Json;
using DTOs;

namespace BlazorApp.Client.Services;

public sealed class TipoOfertaApiClient
{
    private readonly HttpClient _httpClient;

    public TipoOfertaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<TipoOfertaDTO>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<TipoOfertaDTO>>(
            "tipos-oferta") ?? [];
    }

    public async Task<TipoOfertaDTO?> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<TipoOfertaDTO>(
            $"tipos-oferta/{id}");
    }

    public async Task CreateAsync(TipoOfertaDTO tipoOferta)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(
                "tipos-oferta",
                tipoOferta);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(TipoOfertaDTO tipoOferta)
    {
        HttpResponseMessage response =
            await _httpClient.PutAsJsonAsync(
                "tipos-oferta",
                tipoOferta);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage response =
            await _httpClient.DeleteAsync(
                $"tipos-oferta/{id}");

        response.EnsureSuccessStatusCode();
    }
}