using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class TipoOfertaEndpoints
    {
        public static void MapTipoOfertaEndpoints(this WebApplication app)
        {
            app.MapGet("/tiposoferta", async (ITipoOfertaService tipoOfertaService) =>
            {
                var dtos = await tipoOfertaService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllTiposOferta")
            .WithTags("TiposOferta")
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/tiposoferta/{id}", async (int id, ITipoOfertaService tipoOfertaService) =>
            {
                var dto = await tipoOfertaService.GetAsync(id);
                if (dto == null)
                    return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetTipoOferta")
            .WithTags("TiposOferta")
            .WithOpenApi()
            .RequireAuthorization();

            app.MapPost("/tiposoferta", async (TipoOfertaDTO dto, ITipoOfertaService tipoOfertaService) =>
            {
                try
                {
                    var creado = await tipoOfertaService.AddAsync(dto);
                    return Results.Created($"/tiposoferta/{creado.Id}", creado);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddTipoOferta")
            .WithTags("TiposOferta")
            .Produces<TipoOfertaDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);

            app.MapPut("/tiposoferta", async (TipoOfertaDTO dto, ITipoOfertaService tipoOfertaService) =>
            {
                try
                {
                    var found = await tipoOfertaService.UpdateAsync(dto);
                    if (!found)
                        return Results.NotFound();
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateTipoOferta")
            .WithTags("TiposOferta")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);

            app.MapDelete("/tiposoferta/{id}", async (int id, ITipoOfertaService tipoOfertaService) =>
            {
                try
                {
                    var deleted = await tipoOfertaService.DeleteAsync(id);
                    if (!deleted)
                        return Results.NotFound();
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .WithName("DeleteTipoOferta")
            .WithTags("TiposOferta")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);
        }
    }
}
