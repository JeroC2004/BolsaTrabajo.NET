using Application.Services;
using Domain.Model;
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class CurriculumEndpoints
    {
        // Ver o descargar el CV: el administrador siempre; el alumno solo el suyo; la empresa
        // únicamente el de alumnos que se postularon a alguna de sus ofertas.
        private static async Task<bool> PuedeVerAsync(int alumnoId, ClaimsPrincipal user, IPostulacionService postulacionService)
        {
            if (user.EsAdministrador())
                return true;

            if (user.EsAlumno())
                return user.GetAlumnoId() == alumnoId;

            if (user.EsEmpresa())
            {
                int? empresaId = user.GetEmpresaId();
                if (empresaId == null)
                    return false;

                var postulaciones = await postulacionService.GetAllAsync(alumnoId: alumnoId, empresaId: empresaId);
                return postulaciones.Any();
            }

            return false;
        }

        // Cargar o borrar el CV: el administrador o el propio alumno.
        private static bool PuedeModificar(int alumnoId, ClaimsPrincipal user)
        {
            return user.EsAdministrador() || (user.EsAlumno() && user.GetAlumnoId() == alumnoId);
        }

        public static void MapCurriculumEndpoints(this WebApplication app)
        {
            app.MapGet("/alumnos/{id}/cv/info", async (int id, ICurriculumService curriculumService,
                IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
                if (!await PuedeVerAsync(id, user, postulacionService))
                    return Results.Forbid();

                CurriculumDTO? info = await curriculumService.GetInfoAsync(id);
                if (info == null)
                    return Results.NotFound(new { error = "El alumno no tiene un CV cargado." });

                return Results.Ok(info);
            })
            .WithName("GetCurriculumInfo")
            .WithTags("Curriculum")
            .Produces<CurriculumDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/alumnos/{id}/cv", async (int id, ICurriculumService curriculumService,
                IPostulacionService postulacionService, ClaimsPrincipal user) =>
            {
                if (!await PuedeVerAsync(id, user, postulacionService))
                    return Results.Forbid();

                CurriculumArchivoDTO? archivo = await curriculumService.GetArchivoAsync(id);
                if (archivo == null)
                    return Results.NotFound(new { error = "El alumno no tiene un CV cargado." });

                return Results.File(archivo.Contenido, "application/pdf", archivo.NombreArchivo);
            })
            .WithName("DescargarCurriculum")
            .WithTags("Curriculum")
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            // multipart/form-data con el campo "archivo". Se autentica con JWT (no con cookies),
            // por eso no aplica la protección antiforgery.
            app.MapPut("/alumnos/{id}/cv", async (int id, IFormFile archivo, ICurriculumService curriculumService,
                ClaimsPrincipal user) =>
            {
                if (!PuedeModificar(id, user))
                    return Results.Forbid();

                if (archivo.Length == 0)
                    return Results.BadRequest(new { error = "El archivo está vacío." });

                // Se corta antes de copiar el archivo a memoria.
                if (archivo.Length > CurriculumVitae.TamanioMaximoBytes)
                    return Results.BadRequest(new { error = $"El archivo supera el tamaño máximo de {CurriculumVitae.TamanioMaximoBytes / (1024 * 1024)} MB." });

                try
                {
                    using var memoria = new MemoryStream((int)archivo.Length);
                    await archivo.CopyToAsync(memoria);

                    CurriculumDTO dto = await curriculumService.GuardarAsync(id, archivo.FileName, memoria.ToArray());
                    return Results.Ok(dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .DisableAntiforgery()
            .WithName("GuardarCurriculum")
            .WithTags("Curriculum")
            .Produces<CurriculumDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOAlumno);

            app.MapDelete("/alumnos/{id}/cv", async (int id, ICurriculumService curriculumService, ClaimsPrincipal user) =>
            {
                if (!PuedeModificar(id, user))
                    return Results.Forbid();

                bool eliminado = await curriculumService.DeleteAsync(id);
                return eliminado ? Results.NoContent() : Results.NotFound();
            })
            .WithName("EliminarCurriculum")
            .WithTags("Curriculum")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOAlumno);
        }
    }
}
