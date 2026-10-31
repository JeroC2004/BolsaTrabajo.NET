// Guía de estudio: Define las rutas HTTP de Reporte mediante Minimal APIs. Recibe parámetros o DTO, aplica autorización, llama a servicios y traduce los resultados a respuestas HTTP.
using Application.Services;
using System.Security.Claims;

namespace WebAPI
{
    public static class ReporteEndpoints
    {
        // Registra las rutas del recurso y sus manejadores; cada endpoint especifica su respuesta y los permisos requeridos.
        public static void MapReporteEndpoints(this WebApplication app)
        {
            // GET /reportes/postulaciones-por-oferta: Consulta y devuelve los datos solicitados.
            app.MapGet("/reportes/postulaciones-por-oferta",
                async (DateTime? desde, DateTime? hasta, IReporteService reporteService, ClaimsPrincipal user) =>
                {
                    int? empresaId = user.EsEmpresa() ? (user.GetEmpresaId() ?? 0) : null;

                    try
                    {
                        var filas = await reporteService.GetPostulacionesPorOfertaAsync(empresaId, desde, hasta);
                        return Results.Ok(filas);
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.BadRequest(new { error = ex.Message });
                    }
                })
            .WithName("ReportePostulacionesPorOferta")
            .WithTags("Reportes")
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);

            // GET /reportes/ofertas-por-tipo: Consulta y devuelve los datos solicitados.
            app.MapGet("/reportes/ofertas-por-tipo", async (IReporteService reporteService, ClaimsPrincipal user) =>
            {
                int? empresaId = user.EsEmpresa() ? (user.GetEmpresaId() ?? 0) : null;

                var filas = await reporteService.GetOfertasPorTipoAsync(empresaId);
                return Results.Ok(filas);
            })
            .WithName("ReporteOfertasPorTipo")
            .WithTags("Reportes")
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);
        }
    }
}
