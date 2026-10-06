using API.Clients;
using BlazorApp.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();

builder.Services.AddTransient<BearerTokenHandler>();

var apiUrl = new Uri("http://localhost:5183/");

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

builder.Services.AddHttpClient<AuthApiClient>(client => client.BaseAddress = apiUrl);

builder.Services.AddScoped<IAuthService, BlazorAuthService>();
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<BlazorAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(
    serviceProvider =>
        serviceProvider.GetRequiredService<BlazorAuthenticationStateProvider>());

await builder.Build().RunAsync();