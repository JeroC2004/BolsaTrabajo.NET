// Guía de estudio: Guarda y recupera token y nombre de usuario en localStorage mediante interoperabilidad con JavaScript.
using Microsoft.JSInterop;

namespace BlazorApp.Client.Authentication;

public sealed class TokenStorage
{
    private const string TokenKey = "bolsaTrabajoToken";
    private const string UsernameKey = "bolsaTrabajoUsername";

    private readonly IJSRuntime jsRuntime;

    // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
    public TokenStorage(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    // Guarda el token y el nombre de usuario en el almacenamiento local del navegador.
    public async Task SaveAsync(string token, string username)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", UsernameKey, username);
    }

    // Recupera el token de la sesión para autorizar las solicitudes HTTP.
    public async Task<string?> GetTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            TokenKey);
    }

    // Obtiene el nombre de usuario guardado en la sesión.
    public async Task<string?> GetUsernameAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            UsernameKey);
    }

    // Elimina del almacenamiento local los datos de la sesión.
    public async Task ClearAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", UsernameKey);
    }
}