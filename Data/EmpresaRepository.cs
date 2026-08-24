using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly BolsaTrabajoContext context;

        public EmpresaRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Empresa>> GetAllAsync()
        {
            return await context.Empresas.OrderBy(e => e.RazonSocial).ToListAsync();
        }

        public async Task<Empresa?> GetAsync(int id)
        {
            return await context.Empresas.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
