using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Data
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly BolsaTrabajoContext context;

        public ReporteRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OfertaPostulacionesResumen>> GetPostulacionesPorOfertaAsync(
            int? empresaId, DateTime? desde, DateTime? hasta)
        {
            IQueryable<Postulacion> postulaciones = context.Postulaciones;

            if (desde.HasValue)
            {
                DateTime fechaDesde = desde.Value.Date;
                postulaciones = postulaciones.Where(p => p.FechaPostulacion >= fechaDesde);
            }

            if (hasta.HasValue)
            {
                DateTime fechaHasta = hasta.Value.Date;
                postulaciones = postulaciones.Where(p => p.FechaPostulacion.Date <= fechaHasta);
            }

            IQueryable<Oferta> ofertas = context.Ofertas;

            if (empresaId.HasValue)
                ofertas = ofertas.Where(o => o.EmpresaId == empresaId.Value);

            return await ofertas
                .Select(o => new OfertaPostulacionesResumen
                {
                    OfertaId = o.Id,
                    Titulo = o.Titulo,
                    Empresa = o.Empresa!.RazonSocial,
                    Estado = o.Estado,
                    Total = postulaciones.Count(p => p.OfertaId == o.Id),
                    Pendientes = postulaciones.Count(p => p.OfertaId == o.Id && p.Estado == EstadoPostulacion.Pendiente),
                    Aceptadas = postulaciones.Count(p => p.OfertaId == o.Id && p.Estado == EstadoPostulacion.Aceptada),
                    Rechazadas = postulaciones.Count(p => p.OfertaId == o.Id && p.Estado == EstadoPostulacion.Rechazada)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TipoOfertaResumen>> GetOfertasPorTipoAsync(int? empresaId)
        {
            const string sql = @"
                SELECT t.Nombre, COUNT(o.Id) AS Total,
                       COALESCE(SUM(CASE WHEN o.Estado = @estado THEN 1 ELSE 0 END), 0) AS Activas
                FROM TiposOferta AS t
                LEFT JOIN Ofertas AS o ON o.TipoOfertaId = t.Id
                    AND (@empresaId IS NULL OR o.EmpresaId = @empresaId)
                GROUP BY t.Id, t.Nombre
                ORDER BY t.Nombre;";

            await using var connection = new SqlConnection(context.Database.GetConnectionString());
            await connection.OpenAsync();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@empresaId", SqlDbType.Int).Value = (object?)empresaId ?? DBNull.Value;
            command.Parameters.Add("@estado", SqlDbType.NVarChar, 20).Value = nameof(EstadoOferta.Activa);
            await using SqlDataReader reader = await command.ExecuteReaderAsync();
            var filas = new List<TipoOfertaResumen>();
            while (await reader.ReadAsync())
                filas.Add(new TipoOfertaResumen
                {
                    Nombre = reader.GetString(0),
                    Total = reader.GetInt32(1),
                    Activas = reader.GetInt32(2)
                });
            return filas;
        }
    }
}
