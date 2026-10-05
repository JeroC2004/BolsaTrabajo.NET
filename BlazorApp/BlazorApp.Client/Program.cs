using API.Clients;
using BlazorApp.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5183/")
});

builder.Services.AddScoped<IAuthService, BlazorAuthService>();
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<BlazorAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(
    serviceProvider =>
        serviceProvider.GetRequiredService<BlazorAuthenticationStateProvider>());

var host = builder.Build();

IAuthService authService =
    host.Services.GetRequiredService<IAuthService>();

AuthServiceProvider.Register(authService);

await host.RunAsync();
