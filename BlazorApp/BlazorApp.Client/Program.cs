using API.Clients;
using BlazorApp.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();

builder.Services.AddTransient<BearerTokenHandler>();

// WebAssembly obtiene del host la URL configurada mediante la variable de entorno.
using var configurationClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var configuration = await configurationClient.GetFromJsonAsync<Dictionary<string, string>>("configuracion-cliente");
if (configuration == null || !configuration.TryGetValue("apiBaseUrl", out var baseUrl)
    || !Uri.TryCreate(baseUrl, UriKind.Absolute, out var apiUrl)
    || (apiUrl.Scheme != Uri.UriSchemeHttp && apiUrl.Scheme != Uri.UriSchemeHttps))
{
    throw new InvalidOperationException("El host no proporcionó una URL HTTP válida para la API.");
}

builder.Services.AddHttpClient<OfertaApiClient>(client => client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<AlumnoApiClient>(client => client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<CarreraApiClient>(client => client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<EmpresaApiClient>(client => client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<TipoOfertaApiClient>(client => client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<PostulacionApiClient>(client => client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<ReporteApiClient>(client => client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<CurriculumApiClient>(client => client.BaseAddress = apiUrl)
    .AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<AuthApiClient>(client => client.BaseAddress = apiUrl);

builder.Services.AddScoped<IAuthService, BlazorAuthService>();
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<BlazorAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(
    serviceProvider =>
        serviceProvider.GetRequiredService<BlazorAuthenticationStateProvider>());

await builder.Build().RunAsync();
