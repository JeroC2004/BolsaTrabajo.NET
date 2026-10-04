using BlazorApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

WebAssemblyHostBuilder builder =
    WebAssemblyHostBuilder.CreateDefault(args);

string apiBaseUrl =
    builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException(
        "No se configuró ApiBaseUrl.");

builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<BlazorAuthService>();
builder.Services.AddScoped<JwtAuthorizationHandler>();

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    serviceProvider =>
        serviceProvider.GetRequiredService<
            JwtAuthenticationStateProvider>());

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<JwtAuthorizationHandler>();

builder.Services.AddScoped(serviceProvider =>
{
    IHttpClientFactory factory =
        serviceProvider.GetRequiredService<IHttpClientFactory>();

    return factory.CreateClient("Api");
});

builder.Services.AddScoped<OfertaApiClient>();
builder.Services.AddScoped<AlumnoApiClient>();
builder.Services.AddScoped<EmpresaApiClient>();
builder.Services.AddScoped<CarreraApiClient>();
builder.Services.AddScoped<TipoOfertaApiClient>();

await builder.Build().RunAsync();
