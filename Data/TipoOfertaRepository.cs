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
    }
}
