// Guía de estudio: Acceso a datos de TipoOferta mediante Entity Framework Core: construye consultas y persiste cambios usando BolsaTrabajoContext. Separa los detalles de base de datos de los servicios.
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TipoOfertaRepository : ITipoOfertaRepository
    {
        private readonly BolsaTrabajoContext context;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public TipoOfertaRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        // Obtiene la colección de registros en la base de datos mediante el contexto de EF Core.
        public async Task<IEnumerable<TipoOferta>> GetAllAsync()
        {
            return await context.TiposOferta.OrderBy(t => t.Nombre).ToListAsync();
        }

        // Busca un registro por su identificador en la base de datos mediante el contexto de EF Core.
        public async Task<TipoOferta?> GetAsync(int id)
        {
            return await context.TiposOferta.FirstOrDefaultAsync(t => t.Id == id);
        }

        // Agrega un registro en la base de datos mediante el contexto de EF Core.
        public async Task AddAsync(TipoOferta tipoOferta)
        {
            context.TiposOferta.Add(tipoOferta);
            await context.SaveChangesAsync();
        }

        // Actualiza el registro o su estado en la base de datos mediante el contexto de EF Core.
        public async Task<bool> UpdateAsync(TipoOferta tipoOferta)
        {
            var existing = await context.TiposOferta.FindAsync(tipoOferta.Id);
            if (existing == null)
                return false;

            existing.SetNombre(tipoOferta.Nombre);

            await context.SaveChangesAsync();
            return true;
        }

        // Elimina el registro indicado en la base de datos mediante el contexto de EF Core.
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

        // Comprueba relaciones existentes que pueden impedir eliminar un registro utilizado por otros datos.
        public async Task<bool> HasOfertasAsync(int id)
        {
            return await context.Ofertas.AnyAsync(o => o.TipoOfertaId == id);
        }
    }
}
