using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class EmpresaEndpoints
    {
        public static void MapEmpresaEndpoints(this WebApplication app)
        {
            app.MapGet("/empresas/solicitudes", async (IEmpresaService service) => Results.Ok(await service.GetSolicitudesAsync()))
                .WithName("GetSolicitudesEmpresas").WithTags("Empresas").RequireAuthorization(Policies.Administrador);
            app.MapPut("/empresas/solicitudes/{id:int}/aprobar", (int id, IEmpresaService service) => Resolver(id, true, service))
                .WithName("AprobarEmpresa").WithTags("Empresas").RequireAuthorization(Policies.Administrador);
            app.MapPut("/empresas/solicitudes/{id:int}/rechazar", (int id, IEmpresaService service) => Resolver(id, false, service))
                .WithName("RechazarEmpresa").WithTags("Empresas").RequireAuthorization(Policies.Administrador);
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

        private static async Task<IResult> Resolver(int id, bool aprobar, IEmpresaService service)
        {
            try
            {
                return await service.ResolverSolicitudAsync(id, aprobar) ? Results.NoContent()
                    : Results.Conflict(new { error = "La solicitud no existe o ya fue resuelta." });
            }
            catch (InvalidOperationException ex) { return Results.Conflict(new { error = ex.Message }); }
        }
    }
}
