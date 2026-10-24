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

        public async Task AddAsync(Empresa empresa)
        {
            context.Empresas.Add(empresa);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Empresa empresa)
        {
            var existing = await context.Empresas.FindAsync(empresa.Id);
            if (existing == null)
                return false;

            existing.SetRazonSocial(empresa.RazonSocial);
            existing.SetDescripcion(empresa.Descripcion);
            existing.SetRubro(empresa.Rubro);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await context.Empresas.FindAsync(id);
            if (existing == null)
                return false;

            context.Empresas.Remove(existing);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NameExistsAsync(string razonSocial, int? excludeId = null)
        {
            var query = context.Empresas.Where(e => e.RazonSocial.ToLower() == razonSocial.ToLower());
            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<bool> HasOfertasAsync(int id)
        {
            return await context.Ofertas.AnyAsync(o => o.EmpresaId == id);
        }
    }
}
