using Application.Services;
using DTOs;

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
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/empresas/{id}", async (int id, IEmpresaService empresaService) =>
            {
                var dto = await empresaService.GetAsync(id);
                if (dto == null)
                    return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetEmpresa")
            .WithTags("Empresas")
            .WithOpenApi()
            .RequireAuthorization();

            app.MapPost("/empresas", async (EmpresaDTO dto, IEmpresaService empresaService) =>
            {
                try
                {
                    var creado = await empresaService.AddAsync(dto);
                    return Results.Created($"/empresas/{creado.Id}", creado);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddEmpresa")
            .WithTags("Empresas")
            .Produces<EmpresaDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);

            app.MapPut("/empresas", async (EmpresaDTO dto, IEmpresaService empresaService) =>
            {
                try
                {
                    var found = await empresaService.UpdateAsync(dto);
                    if (!found)
                        return Results.NotFound();
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateEmpresa")
            .WithTags("Empresas")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);

            app.MapDelete("/empresas/{id}", async (int id, IEmpresaService empresaService) =>
            {
                try
                {
                    var deleted = await empresaService.DeleteAsync(id);
                    if (!deleted)
                        return Results.NotFound();
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .WithName("DeleteEmpresa")
            .WithTags("Empresas")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);
        }
    }
}
