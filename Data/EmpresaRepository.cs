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
            return await context.Empresas.Where(e => e.EstadoRegistro == EstadoRegistroEmpresa.Aprobada)
                .OrderBy(e => e.RazonSocial).ToListAsync();
        }

        public async Task<Empresa?> GetAsync(int id)
        {
            return await context.Empresas.FirstOrDefaultAsync(e => e.Id == id && e.EstadoRegistro == EstadoRegistroEmpresa.Aprobada);
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

        public async Task<IEnumerable<(Empresa Empresa, string Username, string Email)>> GetSolicitudesAsync()
        {
            var solicitudes = await (from e in context.Empresas.AsNoTracking()
                join u in context.Usuarios on e.Id equals u.EmpresaId
                where e.EstadoRegistro != EstadoRegistroEmpresa.Aprobada && u.Rol == RolUsuario.Empresa
                orderby e.RazonSocial
                select new { Empresa = e, u.Username, u.Email }).ToListAsync();
            return solicitudes.Select(s => (s.Empresa, s.Username, s.Email)).ToList();
        }

        public async Task<bool> ResolverSolicitudAsync(int id, bool aprobar)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            var estado = aprobar ? EstadoRegistroEmpresa.Aprobada : EstadoRegistroEmpresa.Rechazada;
            int modificadas = await context.Empresas.Where(e => e.Id == id && e.EstadoRegistro == EstadoRegistroEmpresa.Pendiente)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.EstadoRegistro, estado));
            if (modificadas == 0) return false;
            int usuarios = await context.Usuarios.Where(u => u.EmpresaId == id && u.Rol == RolUsuario.Empresa)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.Activo, aprobar));
            if (usuarios == 0) throw new InvalidOperationException("La solicitud no tiene una cuenta asociada.");
            await transaction.CommitAsync();
            return true;
        }
    }
}
