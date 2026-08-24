using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/login", async (LoginRequest request, IAuthService authService) =>
            {
                var response = await authService.LoginAsync(request);

                if (response == null)
                    return Results.Unauthorized();

                return Results.Ok(response);
            })
            .WithName("Login")
            .WithTags("Auth")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi()
            .AllowAnonymous(); // Único endpoint público: hace falta para poder loguearse
        }
    }
}
