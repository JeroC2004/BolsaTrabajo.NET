using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class OfertaRepository : IOfertaRepository
    {
        private readonly BolsaTrabajoContext context;

        public OfertaRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Oferta oferta)
        {
            context.Ofertas.Add(oferta);
            await context.SaveChangesAsync();
        }

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

        public async Task<Oferta?> GetAsync(int id)
        {
            return await context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .Include(o => o.Requisitos)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Oferta>> GetAllAsync()
        {
            return await context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .Include(o => o.Requisitos)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Oferta oferta)
        {
            var existing = await context.Ofertas
                .Include(o => o.Requisitos)
                .FirstOrDefaultAsync(o => o.Id == oferta.Id);

            if (existing == null)
                return false;

            existing.SetFechaHasta(oferta.FechaHasta);
            existing.SetFechaDesde(oferta.FechaDesde);
            existing.SetTitulo(oferta.Titulo);
            existing.SetTipoVinculo(oferta.TipoVinculo);
            existing.SetDetalle(oferta.Detalle);
            existing.SetEstado(oferta.Estado);
            existing.SetEmpresaId(oferta.EmpresaId);
            existing.SetTipoOfertaId(oferta.TipoOfertaId);

            SincronizarRequisitos(existing, oferta.Requisitos);

            // Cabecera y requisitos se guardan juntos en un único SaveChanges (una transacción).
            await context.SaveChangesAsync();
            return true;
        }

        // Deja en 'existing' exactamente los requisitos recibidos: actualiza las líneas que
        // siguen (mismo Id), agrega las nuevas (Id = 0 o Id que no pertenece a esta oferta)
        // y elimina las que ya no vienen.
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

        public async Task<IEnumerable<Oferta>> GetByCriteriaAsync(OfertaCriteria criteria)
        {
            string searchTerm = criteria.Texto.ToLower();

            return await context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .Include(o => o.Requisitos)
                .Where(o =>
                    o.Titulo.ToLower().Contains(searchTerm) ||
                    o.Detalle.ToLower().Contains(searchTerm))
                .OrderByDescending(o => o.FechaDesde)
                .ToListAsync();
        }
    }
}
