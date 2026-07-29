using Application.Services;

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
            .WithOpenApi();
        }
    }
}
