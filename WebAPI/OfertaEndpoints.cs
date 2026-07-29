using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class OfertaEndpoints
    {
        public static void MapOfertaEndpoints(this WebApplication app)
        {
            app.MapGet("/ofertas/{id}", async (int id, IOfertaService ofertaService) =>
            {
                OfertaDTO? dto = await ofertaService.GetAsync(id);

                if (dto == null)
                    return Results.NotFound();

                return Results.Ok(dto);
            })
            .WithName("GetOferta")
            .WithTags("Ofertas")
            .Produces<OfertaDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/ofertas", async (IOfertaService ofertaService) =>
            {
                var dtos = await ofertaService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllOfertas")
            .WithTags("Ofertas")
            .Produces<List<OfertaDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/ofertas", async (OfertaDTO dto, IOfertaService ofertaService) =>
            {
                try
                {
                    OfertaDTO ofertaDTO = await ofertaService.AddAsync(dto);
                    return Results.Created($"/ofertas/{ofertaDTO.Id}", ofertaDTO);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddOferta")
            .WithTags("Ofertas")
            .Produces<OfertaDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/ofertas", async (OfertaDTO dto, IOfertaService ofertaService) =>
            {
                try
                {
                    var found = await ofertaService.UpdateAsync(dto);

                    if (!found)
                        return Results.NotFound();

                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateOferta")
            .WithTags("Ofertas")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/ofertas/{id}", async (int id, IOfertaService ofertaService) =>
            {
                var deleted = await ofertaService.DeleteAsync(id);

                if (!deleted)
                    return Results.NotFound();

                return Results.NoContent();
            })
            .WithName("DeleteOferta")
            .WithTags("Ofertas")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/ofertas/criteria", async (string texto, IOfertaService ofertaService) =>
            {
                try
                {
                    var criteria = new OfertaCriteriaDTO { Texto = texto };
                    var ofertas = await ofertaService.GetByCriteriaAsync(criteria);
                    return Results.Ok(ofertas);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetOfertasByCriteria")
            .WithTags("Ofertas")
            .WithOpenApi();
        }
    }
}
