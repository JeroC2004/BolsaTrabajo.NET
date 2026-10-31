// Guía de estudio: Acceso a datos de Oferta mediante Entity Framework Core: construye consultas y persiste cambios usando BolsaTrabajoContext. Separa los detalles de base de datos de los servicios.
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class OfertaRepository : IOfertaRepository
    {
        private readonly BolsaTrabajoContext context;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public OfertaRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        // Agrega un registro en la base de datos mediante el contexto de EF Core.
        public async Task AddAsync(Oferta oferta)
        {
            context.Ofertas.Add(oferta);
            await context.SaveChangesAsync();
        }

        // Elimina el registro indicado en la base de datos mediante el contexto de EF Core.
        public async Task<bool> DeleteAsync(int id)
        {
            var oferta = await context.Ofertas.FindAsync(id);
            if (oferta != null)
            {
                context.Ofertas.Remove(oferta);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Busca un registro por su identificador en la base de datos mediante el contexto de EF Core.
        public async Task<Oferta?> GetAsync(int id)
        {
            return await context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .Include(o => o.Requisitos)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // Obtiene la colección de registros en la base de datos mediante el contexto de EF Core.
        public async Task<IEnumerable<Oferta>> GetAllAsync()
        {
            return await context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .Include(o => o.Requisitos)
                .ToListAsync();
        }

        // Actualiza el registro o su estado en la base de datos mediante el contexto de EF Core.
        public async Task<bool> UpdateAsync(Oferta oferta)
        {
            var existing = await context.Ofertas
                .Include(o => o.Requisitos)
                .FirstOrDefaultAsync(o => o.Id == oferta.Id);

            if (existing == null)
                return false;

            existing.SetFechas(oferta.FechaDesde, oferta.FechaHasta);
            existing.SetTitulo(oferta.Titulo);
            existing.SetTipoVinculo(oferta.TipoVinculo);
            existing.SetDetalle(oferta.Detalle);
            existing.SetEstado(oferta.Estado);
            existing.SetEmpresaId(oferta.EmpresaId);
            existing.SetTipoOfertaId(oferta.TipoOfertaId);

            SincronizarRequisitos(existing, oferta.Requisitos);

            await context.SaveChangesAsync();
            return true;
        }

        // Compara requisitos actuales y recibidos: modifica los conservados, agrega los nuevos y elimina los que ya no están.
        private void SincronizarRequisitos(Oferta existing, ICollection<RequisitoOferta> recibidos)
        {
            var actuales = existing.Requisitos.ToDictionary(r => r.Id);
            var conservados = new HashSet<int>();

            foreach (var recibido in recibidos)
            {
                if (recibido.Id != 0 && actuales.TryGetValue(recibido.Id, out var actual))
                {
                    actual.Descripcion = recibido.Descripcion;
                    actual.EsExcluyente = recibido.EsExcluyente;
                    conservados.Add(actual.Id);
                }
                else
                {
                    existing.Requisitos.Add(new RequisitoOferta
                    {
                        Descripcion = recibido.Descripcion,
                        EsExcluyente = recibido.EsExcluyente
                    });
                }
            }

            var eliminados = actuales.Values.Where(r => !conservados.Contains(r.Id)).ToList();
            context.RequisitosOferta.RemoveRange(eliminados);
        }

        // Consulta registros con los criterios de búsqueda recibidos en la base de datos mediante el contexto de EF Core.
        public async Task<IEnumerable<Oferta>> GetByCriteriaAsync(OfertaCriteria criteria)
        {
            var query = context.Ofertas.AsNoTracking()
                .Include(o => o.Empresa).Include(o => o.TipoOferta).Include(o => o.Requisitos)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(criteria.Texto))
            {
                string texto = criteria.Texto.ToLower();
                query = query.Where(o => o.Titulo.ToLower().Contains(texto) || o.Detalle.ToLower().Contains(texto));
            }
            if (criteria.TipoOfertaId.HasValue)
                query = query.Where(o => o.TipoOfertaId == criteria.TipoOfertaId.Value);
            if (criteria.Estado.HasValue)
                query = query.Where(o => o.Estado == criteria.Estado.Value);
            if (criteria.TipoVinculo.HasValue)
                query = query.Where(o => o.TipoVinculo == criteria.TipoVinculo.Value);
            if (criteria.Desde.HasValue)
                query = query.Where(o => o.FechaDesde >= criteria.Desde.Value);
            if (criteria.Hasta.HasValue)
                query = query.Where(o => o.FechaDesde.Date <= criteria.Hasta.Value);
            return await query.OrderByDescending(o => o.FechaDesde).ThenBy(o => o.Id).ToListAsync();
        }
    }
}
