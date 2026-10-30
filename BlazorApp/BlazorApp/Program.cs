using API.Clients;
using BlazorApp.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Publicar solamente la URL de la API, nunca la configuración completa del host.
app.MapGet("/configuracion-cliente", (HttpContext context) =>
{
    context.Response.Headers.CacheControl = "no-store";
    return Results.Ok(new { ApiBaseUrl = ApiConfiguration.BaseUrl });
}).AllowAnonymous();

// El host entrega la aplicación sin prerenderizar las páginas protegidas.
// El token está en localStorage: AuthorizeRouteView valida los roles en el
// cliente y la API valida el JWT y los permisos al acceder a los datos.
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp.Client._Imports).Assembly)
    .AllowAnonymous();

app.Run();
