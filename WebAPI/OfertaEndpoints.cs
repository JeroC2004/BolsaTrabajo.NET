// Guía de estudio: Define las rutas HTTP de Oferta mediante Minimal APIs. Recibe parámetros o DTO, aplica autorización, llama a servicios y traduce los resultados a respuestas HTTP.
using Application.Services;
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class OfertaEndpoints
    {
        // Limita las ofertas visibles: una empresa consulta las propias y un alumno solamente las activas.
        private static IEnumerable<OfertaDTO> FiltrarPorRol(IEnumerable<OfertaDTO> ofertas, ClaimsPrincipal user)
        {
            if (user.EsEmpresa())
            {
                int? empresaId = user.GetEmpresaId();
                return ofertas.Where(o => o.EmpresaId == empresaId);
            }

            if (user.EsAlumno())
                return ofertas.Where(o => o.Estado == nameof(Domain.Model.EstadoOferta.Activa));

            return ofertas;
        }

        // Registra las rutas del recurso y sus manejadores; cada endpoint especifica su respuesta y los permisos requeridos.
        public static void MapOfertaEndpoints(this WebApplication app)
        {
            // GET /ofertas/{id}: Consulta y devuelve los datos solicitados.
            app.MapGet("/ofertas/{id}", async (int id, IOfertaService ofertaService, ClaimsPrincipal user) =>
            {
                OfertaDTO? dto = await ofertaService.GetAsync(id);
                if (dto == null)
                    return Results.NotFound();
                if (user.EsEmpresa() && dto.EmpresaId != user.GetEmpresaId())
                    return Results.Forbid();
                if (user.EsAlumno() && dto.Estado != nameof(Domain.Model.EstadoOferta.Activa))
                    return Results.Forbid();
                return Results.Ok(dto);
            })
            .WithName("GetOferta")
            .WithTags("Ofertas")
            .Produces<OfertaDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            // GET /ofertas: Consulta y devuelve los datos solicitados.
            app.MapGet("/ofertas", async (IOfertaService ofertaService, ClaimsPrincipal user) =>
            {
                var dtos = await ofertaService.GetAllAsync();
                return Results.Ok(FiltrarPorRol(dtos, user).ToList());
            })
            .WithName("GetAllOfertas")
            .WithTags("Ofertas")
            .Produces<List<OfertaDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization();

            // POST /ofertas: Procesa la creación o la acción recibida en la solicitud.
            app.MapPost("/ofertas", async (OfertaDTO dto, IOfertaService ofertaService, ClaimsPrincipal user) =>
            {
                if (user.EsEmpresa())
                    dto.EmpresaId = user.GetEmpresaId() ?? 0;

                try
                {
                    OfertaDTO ofertaDTO = await ofertaService.AddAsync(dto);
                    return Results.Created($"/ofertas/{ofertaDTO.Id}", ofertaDTO);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddOferta")
            .WithTags("Ofertas")
            .Produces<OfertaDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);

            // PUT /ofertas: Procesa la actualización de los datos recibidos.
            app.MapPut("/ofertas", async (OfertaDTO dto, IOfertaService ofertaService, ClaimsPrincipal user) =>
            {
                if (user.EsEmpresa())
                {
                    var existente = await ofertaService.GetAsync(dto.Id);
                    if (existente == null)
                        return Results.NotFound();
                    if (existente.EmpresaId != user.GetEmpresaId())
                        return Results.Forbid();
                    dto.EmpresaId = existente.EmpresaId;
                }

                try
                {
                    var found = await ofertaService.UpdateAsync(dto);
                    if (!found)
                        return Results.NotFound();
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateOferta")
            .WithTags("Ofertas")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);

            // DELETE /ofertas/{id}: Solicita la eliminación del recurso indicado.
            app.MapDelete("/ofertas/{id}", async (int id, IOfertaService ofertaService, ClaimsPrincipal user) =>
            {
                if (user.EsEmpresa())
                {
                    var existente = await ofertaService.GetAsync(id);
                    if (existente == null)
                        return Results.NotFound();
                    if (existente.EmpresaId != user.GetEmpresaId())
                        return Results.Forbid();
                }

                var deleted = await ofertaService.DeleteAsync(id);
                if (!deleted)
                    return Results.NotFound();
                return Results.NoContent();
            })
            .WithName("DeleteOferta")
            .WithTags("Ofertas")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(Policies.AdministradorOEmpresa);

            // GET /ofertas/criteria: Consulta y devuelve los datos solicitados.
            app.MapGet("/ofertas/criteria", async (IOfertaService ofertaService, ClaimsPrincipal user,
                string? texto = null, int? tipoOfertaId = null, string? estado = null,
                string? tipoVinculo = null, DateTime? desde = null, DateTime? hasta = null) =>
            {
                try
                {
                    var criteria = new OfertaCriteriaDTO
                    {
                        Texto = texto ?? string.Empty,
                        TipoOfertaId = tipoOfertaId,
                        Estado = estado,
                        TipoVinculo = tipoVinculo,
                        Desde = desde,
                        Hasta = hasta
                    };
                    var ofertas = await ofertaService.GetByCriteriaAsync(criteria);
                    return Results.Ok(FiltrarPorRol(ofertas, user).ToList());
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetOfertasByCriteria")
            .WithTags("Ofertas")
            .WithOpenApi()
            .RequireAuthorization();
        }
    }
}
