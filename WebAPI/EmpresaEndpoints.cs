using Application.Services;

namespace WebAPI
{
    public static class EmpresaEndpoints
    {
        public static void MapEmpresaEndpoints(this WebApplication app)
        {
            app.MapGet("/empresas", async (IEmpresaService empresaService) =>
            {
                var dtos = await empresaService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllEmpresas")
            .WithTags("Empresas")
            .WithOpenApi();

            app.MapGet("/empresas/{id}", async (int id, IEmpresaService empresaService) =>
            {
                var dto = await empresaService.GetAsync(id);
                if (dto == null)
                    return Results.NotFound();

                return Results.Ok(dto);
            })
            .WithName("GetEmpresa")
            .WithTags("Empresas")
            .WithOpenApi();
        }
    }
}
