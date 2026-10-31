// Guía de estudio: Define las rutas HTTP de Empresa mediante Minimal APIs. Recibe parámetros o DTO, aplica autorización, llama a servicios y traduce los resultados a respuestas HTTP.
using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class EmpresaEndpoints
    {
        // Registra las rutas del recurso y sus manejadores; cada endpoint especifica su respuesta y los permisos requeridos.
        public static void MapEmpresaEndpoints(this WebApplication app)
        {
            // GET /empresas: Consulta y devuelve los datos solicitados.
            app.MapGet("/empresas", async (IEmpresaService empresaService) =>
            {
                var dtos = await empresaService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllEmpresas")
            .WithTags("Empresas")
            .WithOpenApi()
            .RequireAuthorization();

            // GET /empresas/{id}: Consulta y devuelve los datos solicitados.
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

            // POST /empresas: Procesa la creación o la acción recibida en la solicitud.
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

            // PUT /empresas: Procesa la actualización de los datos recibidos.
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

            // DELETE /empresas/{id}: Solicita la eliminación del recurso indicado.
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
