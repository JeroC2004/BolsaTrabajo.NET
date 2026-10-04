using Microsoft.JSInterop;

namespace BlazorApp.Client.Services;

public sealed class TokenStorage
{
    private const string TokenKey = "bolsa-trabajo-token";
    private const string UsernameKey = "bolsa-trabajo-username";

    private readonly IJSRuntime _jsRuntime;

    public TokenStorage(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetAsync(string token, string username)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UsernameKey, username);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            TokenKey);
    }

    public async Task<string?> GetUsernameAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            UsernameKey);
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UsernameKey);
    }
}