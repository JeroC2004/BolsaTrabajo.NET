using Application.Services;
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class AlumnoEndpoints
    {
        public static void MapAlumnoEndpoints(this WebApplication app)
        {
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
