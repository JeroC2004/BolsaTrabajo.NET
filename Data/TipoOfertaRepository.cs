using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TipoOfertaRepository : ITipoOfertaRepository
    {
        private readonly BolsaTrabajoContext context;

        public TipoOfertaRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TipoOferta>> GetAllAsync()
        {
            return await context.TiposOferta.OrderBy(t => t.Nombre).ToListAsync();
        }

        public async Task<TipoOferta?> GetAsync(int id)
        {
            return await context.TiposOferta.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TipoOferta tipoOferta)
        {
            context.TiposOferta.Add(tipoOferta);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(TipoOferta tipoOferta)
        {
            var existing = await context.TiposOferta.FindAsync(tipoOferta.Id);
            if (existing == null)
                return false;

            existing.SetNombre(tipoOferta.Nombre);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await context.TiposOferta.FindAsync(id);
            if (existing == null)
                return false;

            context.TiposOferta.Remove(existing);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NameExistsAsync(string nombre, int? excludeId = null)
        {
            var query = context.TiposOferta.Where(t => t.Nombre.ToLower() == nombre.ToLower());
            if (excludeId.HasValue)
                query = query.Where(t => t.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<bool> HasOfertasAsync(int id)
        {
            return await context.Ofertas.AnyAsync(o => o.TipoOfertaId == id);
        }
    }
}
