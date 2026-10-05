using Microsoft.JSInterop;

namespace BlazorApp.Client.Authentication;

public sealed class TokenStorage
{
    private const string TokenKey = "bolsaTrabajoToken";
    private const string UsernameKey = "bolsaTrabajoUsername";

    private readonly IJSRuntime jsRuntime;

    public TokenStorage(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task SaveAsync(string token, string username)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", UsernameKey, username);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            TokenKey);
    }

    public async Task<string?> GetUsernameAsync()
    {
        return await jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            UsernameKey);
    }

    public async Task ClearAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", UsernameKey);
    }
}