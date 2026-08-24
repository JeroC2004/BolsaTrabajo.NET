using Application.Services;

namespace WebAPI
{
    public static class CarreraEndpoints
    {
        public static void MapCarreraEndpoints(this WebApplication app)
        {
            app.MapGet("/carreras", async (ICarreraService carreraService) =>
            {
                var dtos = await carreraService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllCarreras")
            .WithTags("Carreras")
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/carreras/{id}", async (int id, ICarreraService carreraService) =>
            {
                var dto = await carreraService.GetAsync(id);
                if (dto == null)
                    return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetCarrera")
            .WithTags("Carreras")
            .WithOpenApi()
            .RequireAuthorization();
        }
    }
}
