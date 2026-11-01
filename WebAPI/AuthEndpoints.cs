using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/register/empresa", async (EmpresaRegistroRequest request, IAuthService service) =>
            {
                try
                {
                    await service.RegisterEmpresaAsync(request);
                    return Results.Created("/auth/login", new { message = "Solicitud enviada. Podrá ingresar cuando el administrador la apruebe." });
                }
                catch (ArgumentException ex) { return Results.BadRequest(new { error = ex.Message }); }
                catch (InvalidOperationException ex) { return Results.Conflict(new { error = ex.Message }); }
            }).WithName("RegisterEmpresa").WithTags("Auth").AllowAnonymous();
            app.MapGet("/auth/carreras", async (ICarreraService service) =>
                Results.Ok(await service.GetAllAsync()))
                .WithTags("Auth").AllowAnonymous();

            app.MapPost(
                "/auth/login",
                async (
                    LoginRequest request,
                    IAuthService authService) =>
                {
                    LoginResponse? response =
                        await authService.LoginAsync(request);

                    if (response == null)
                        return Results.Unauthorized();

                    return Results.Ok(response);
                })
                .WithName("Login")
                .WithTags("Auth")
                .Produces<LoginResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .AllowAnonymous();

            app.MapPost(
                "/auth/register",
                async (
                    RegisterRequest request,
                    IAuthService authService) =>
                {
                    if (string.IsNullOrWhiteSpace(request.Username) ||
                        string.IsNullOrWhiteSpace(request.NomAlumno) ||
                        string.IsNullOrWhiteSpace(request.ApeAlumno) ||
                        string.IsNullOrWhiteSpace(request.Email) ||
                        string.IsNullOrWhiteSpace(request.Legajo) ||
                        string.IsNullOrWhiteSpace(request.Dni) ||
                        string.IsNullOrWhiteSpace(request.Plan) ||
                        string.IsNullOrWhiteSpace(request.Password) ||
                        string.IsNullOrWhiteSpace(request.ConfirmPassword))
                    {
                        return Results.BadRequest(
                            "Todos los campos obligatorios deben estar completos.");
                    }

                    if (request.Password != request.ConfirmPassword)
                    {
                        return Results.BadRequest(
                            "Las contraseñas no coinciden.");
                    }

                    try
                    {
                        await authService.RegisterAsync(request);

                        return Results.Created(
                            "/auth/login",
                            new
                            {
                                message = "El alumno fue registrado correctamente."
                            });
                    }
                    catch (InvalidOperationException exception)
                    {
                        return Results.Conflict(exception.Message);
                    }
                    catch (ArgumentException exception)
                    {
                        return Results.BadRequest(exception.Message);
                    }
                })
                .WithName("Register")
                .WithTags("Auth")
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status409Conflict)
                .AllowAnonymous();
        }
    }
}
