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
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Oferta>> GetAllAsync()
        {
            return await context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Oferta oferta)
        {
            var existing = await context.Ofertas.FindAsync(oferta.Id);
            if (existing != null)
            {
                // Fecha hasta se actualiza primero para que la revalidación cruzada
                // en SetFechaDesde no rechace un rango que en realidad es válido.
                existing.SetFechaHasta(oferta.FechaHasta);
                existing.SetFechaDesde(oferta.FechaDesde);
                existing.SetTitulo(oferta.Titulo);
                existing.SetTipoVinculo(oferta.TipoVinculo);
                existing.SetDetalle(oferta.Detalle);
                existing.SetRequisitos(oferta.Requisitos);
                existing.SetEstado(oferta.Estado);
                existing.SetEmpresaId(oferta.EmpresaId);
                existing.SetTipoOfertaId(oferta.TipoOfertaId);

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Oferta>> GetByCriteriaAsync(OfertaCriteria criteria)
        {
            string searchTerm = criteria.Texto.ToLower();

            return await context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .Where(o =>
                    o.Titulo.ToLower().Contains(searchTerm) ||
                    o.Detalle.ToLower().Contains(searchTerm))
                .OrderByDescending(o => o.FechaDesde)
                .ToListAsync();
        }
    }
}
