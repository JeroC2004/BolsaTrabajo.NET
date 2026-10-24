using Application.Services;
using DTOs;

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

            app.MapPost("/carreras", async (CarreraDTO dto, ICarreraService carreraService) =>
            {
                try
                {
                    var creado = await carreraService.AddAsync(dto);
                    return Results.Created($"/carreras/{creado.Id}", creado);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddCarrera")
            .WithTags("Carreras")
            .Produces<CarreraDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);

            app.MapPut("/carreras", async (CarreraDTO dto, ICarreraService carreraService) =>
            {
                try
                {
                    var found = await carreraService.UpdateAsync(dto);
                    if (!found)
                        return Results.NotFound();
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateCarrera")
            .WithTags("Carreras")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);

            app.MapDelete("/carreras/{id}", async (int id, ICarreraService carreraService) =>
            {
                try
                {
                    var deleted = await carreraService.DeleteAsync(id);
                    if (!deleted)
                        return Results.NotFound();
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .WithName("DeleteCarrera")
            .WithTags("Carreras")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);
        }
    }
}
