// Guía de estudio: Acceso a datos de Empresa mediante Entity Framework Core: construye consultas y persiste cambios usando BolsaTrabajoContext. Separa los detalles de base de datos de los servicios.
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly BolsaTrabajoContext context;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public EmpresaRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        // Obtiene la colección de registros en la base de datos mediante el contexto de EF Core.
        public async Task<IEnumerable<Empresa>> GetAllAsync()
        {
            return await context.Empresas.OrderBy(e => e.RazonSocial).ToListAsync();
        }

        // Busca un registro por su identificador en la base de datos mediante el contexto de EF Core.
        public async Task<Empresa?> GetAsync(int id)
        {
            return await context.Empresas.FirstOrDefaultAsync(e => e.Id == id);
        }

        // Agrega un registro en la base de datos mediante el contexto de EF Core.
        public async Task AddAsync(Empresa empresa)
        {
            context.Empresas.Add(empresa);
            await context.SaveChangesAsync();
        }

        // Actualiza el registro o su estado en la base de datos mediante el contexto de EF Core.
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

        // Elimina el registro indicado en la base de datos mediante el contexto de EF Core.
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

        // Comprueba relaciones existentes que pueden impedir eliminar un registro utilizado por otros datos.
        public async Task<bool> HasOfertasAsync(int id)
        {
            return await context.Ofertas.AnyAsync(o => o.EmpresaId == id);
        }
    }
}
