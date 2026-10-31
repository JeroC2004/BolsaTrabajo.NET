// Guía de estudio: Define las rutas HTTP de Postulacion mediante Minimal APIs. Recibe parámetros o DTO, aplica autorización, llama a servicios y traduce los resultados a respuestas HTTP.
using Application.Services;
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class PostulacionEndpoints
    {
        // Comprueba que el usuario pueda consultar la postulación según su rol y su relación con alumno u oferta.
        private static bool PuedeVer(PostulacionDTO postulacion, ClaimsPrincipal user)
        {
            if (user.EsAlumno())
                return postulacion.AlumnoId == user.GetAlumnoId();

            if (user.EsEmpresa())
                return postulacion.EmpresaId == user.GetEmpresaId();

            return true;
        }

        // Registra las rutas del recurso y sus manejadores; cada endpoint especifica su respuesta y los permisos requeridos.
        public static void MapPostulacionEndpoints(this WebApplication app)
        {
            // GET /postulaciones: Consulta y devuelve los datos solicitados.
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

            // GET /postulaciones/{id}: Consulta y devuelve los datos solicitados.
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

            // POST /postulaciones: Procesa la creación o la acción recibida en la solicitud.
            app.MapPost("/postulaciones", async (PostulacionDTO dto, IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
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

            // PUT /postulaciones: Procesa la actualización de los datos recibidos.
            app.MapPut("/postulaciones", async (PostulacionDTO dto, IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
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
            .RequireAuthorization(Policies.Empresa);

            // DELETE /postulaciones/{id}: Solicita la eliminación del recurso indicado.
            app.MapDelete("/postulaciones/{id}", async (int id, IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
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
