using Application.Services;
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class PostulacionEndpoints
    {
        // Cada perfil solo ve lo suyo: el alumno sus postulaciones, la empresa las de sus ofertas.
        private static bool PuedeVer(PostulacionDTO postulacion, ClaimsPrincipal user)
        {
            if (user.EsAlumno())
                return postulacion.AlumnoId == user.GetAlumnoId();

            if (user.EsEmpresa())
                return postulacion.EmpresaId == user.GetEmpresaId();

            return true;
        }

        public static void MapPostulacionEndpoints(this WebApplication app)
        {
            app.MapGet("/postulaciones", async (int? ofertaId, IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
                int? alumnoId = user.EsAlumno() ? (user.GetAlumnoId() ?? 0) : null;
                int? empresaId = user.EsEmpresa() ? (user.GetEmpresaId() ?? 0) : null;

                var dtos = await postulacionService.GetAllAsync(alumnoId, empresaId, ofertaId);
                return Results.Ok(dtos);
            })
            .WithName("GetAllPostulaciones")
            .WithTags("Postulaciones")
            .Produces<List<PostulacionDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/postulaciones/{id}", async (int id, IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
                var dto = await postulacionService.GetAsync(id);
                if (dto == null)
                    return Results.NotFound();

                if (!PuedeVer(dto, user))
                    return Results.Forbid();

                return Results.Ok(dto);
            })
            .WithName("GetPostulacion")
            .WithTags("Postulaciones")
            .Produces<PostulacionDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapPost("/postulaciones", async (PostulacionDTO dto, IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
                // El alumno solo puede postularse él mismo.
                if (user.EsAlumno())
                    dto.AlumnoId = user.GetAlumnoId() ?? 0;

                try
                {
                    var creada = await postulacionService.AddAsync(dto);
                    return Results.Created($"/postulaciones/{creada.Id}", creada);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddPostulacion")
            .WithTags("Postulaciones")
            .Produces<PostulacionDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOAlumno);

            app.MapPut("/postulaciones", async (PostulacionDTO dto, IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
                // La empresa solo puede cambiar el estado de las postulaciones a sus ofertas.
                if (user.EsEmpresa())
                {
                    var existente = await postulacionService.GetAsync(dto.Id);
                    if (existente == null)
                        return Results.NotFound();
                    if (!PuedeVer(existente, user))
                        return Results.Forbid();
                }

                try
                {
                    var found = await postulacionService.UpdateAsync(dto);
                    if (!found)
                        return Results.NotFound();
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdatePostulacion")
            .WithTags("Postulaciones")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);

            app.MapDelete("/postulaciones/{id}", async (int id, IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
                // El alumno solo puede retirar sus propias postulaciones.
                if (user.EsAlumno())
                {
                    var existente = await postulacionService.GetAsync(id);
                    if (existente == null)
                        return Results.NotFound();
                    if (!PuedeVer(existente, user))
                        return Results.Forbid();
                }

                var deleted = await postulacionService.DeleteAsync(id);
                if (!deleted)
                    return Results.NotFound();
                return Results.NoContent();
            })
            .WithName("DeletePostulacion")
            .WithTags("Postulaciones")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOAlumno);
        }
    }
}
