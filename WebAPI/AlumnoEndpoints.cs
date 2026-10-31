// Guía de estudio: Define las rutas HTTP de Alumno mediante Minimal APIs. Recibe parámetros o DTO, aplica autorización, llama a servicios y traduce los resultados a respuestas HTTP.
using Application.Services;
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class AlumnoEndpoints
    {
        // Registra las rutas del recurso y sus manejadores; cada endpoint especifica su respuesta y los permisos requeridos.
        public static void MapAlumnoEndpoints(this WebApplication app)
        {
            // GET /alumnos/{id}: Consulta y devuelve los datos solicitados.
            app.MapGet("/alumnos/{id}", async (int id, IAlumnoService alumnoService, ClaimsPrincipal user) =>
            {
                if (user.EsAlumno() && user.GetAlumnoId() != id)
                    return Results.Forbid();

                AlumnoDTO? dto = await alumnoService.GetAsync(id);

                if (dto == null)
                    return Results.NotFound();

                return Results.Ok(dto);
            })
            .WithName("GetAlumno")
            .WithTags("Alumnos")
            .Produces<AlumnoDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            // GET /alumnos: Consulta y devuelve los datos solicitados.
            app.MapGet("/alumnos", async (IAlumnoService alumnoService) =>
            {
                var dtos = await alumnoService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllAlumnos")
            .WithTags("Alumnos")
            .Produces<List<AlumnoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);

            // POST /alumnos: Procesa la creación o la acción recibida en la solicitud.
            app.MapPost("/alumnos", async (AlumnoDTO dto, IAlumnoService alumnoService) =>
            {
                try
                {
                    AlumnoDTO alumnoDTO = await alumnoService.AddAsync(dto);
                    return Results.Created($"/alumnos/{alumnoDTO.Id}", alumnoDTO);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddAlumno")
            .WithTags("Alumnos")
            .Produces<AlumnoDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);

            // PUT /alumnos: Procesa la actualización de los datos recibidos.
            app.MapPut("/alumnos", async (AlumnoDTO dto, IAlumnoService alumnoService, ClaimsPrincipal user) =>
            {
                if (user.EsAlumno() && user.GetAlumnoId() != dto.Id)
                    return Results.Forbid();

                try
                {
                    var found = await alumnoService.UpdateAsync(dto);

                    if (!found)
                        return Results.NotFound();

                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateAlumno")
            .WithTags("Alumnos")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOAlumno);

            // DELETE /alumnos/{id}: Solicita la eliminación del recurso indicado.
            app.MapDelete("/alumnos/{id}", async (int id, IAlumnoService alumnoService) =>
            {
                var deleted = await alumnoService.DeleteAsync(id);

                if (!deleted)
                    return Results.NotFound();

                return Results.NoContent();
            })
            .WithName("DeleteAlumno")
            .WithTags("Alumnos")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Policies.Administrador);

            // GET /alumnos/criteria: Consulta y devuelve los datos solicitados.
            app.MapGet("/alumnos/criteria", async (string texto, IAlumnoService alumnoService) =>
            {
                try
                {
                    var criteria = new AlumnoCriteriaDTO { Texto = texto };
                    var alumnos = await alumnoService.GetByCriteriaAsync(criteria);
                    return Results.Ok(alumnos);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetAlumnosByCriteria")
            .WithTags("Alumnos")
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);
        }
    }
}
