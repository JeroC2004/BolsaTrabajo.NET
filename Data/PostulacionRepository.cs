using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class PostulacionRepository : IPostulacionRepository
    {
        private readonly BolsaTrabajoContext context;

        public PostulacionRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        private IQueryable<Postulacion> ConDetalle()
        {
            return context.Postulaciones
                .Include(p => p.Alumno).ThenInclude(a => a!.Carrera)
                .Include(p => p.Oferta).ThenInclude(o => o!.Empresa);
        }

        public async Task AddAsync(Postulacion postulacion)
        {
            context.Postulaciones.Add(postulacion);
            await context.SaveChangesAsync();
        }

        public async Task<Postulacion?> GetAsync(int id)
        {
            return await ConDetalle().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Postulacion>> GetAllAsync(int? alumnoId = null, int? empresaId = null, int? ofertaId = null)
        {
            var query = ConDetalle();

            if (alumnoId.HasValue)
                query = query.Where(p => p.AlumnoId == alumnoId.Value);

            if (empresaId.HasValue)
                query = query.Where(p => p.Oferta!.EmpresaId == empresaId.Value);

            if (ofertaId.HasValue)
                query = query.Where(p => p.OfertaId == ofertaId.Value);

            return await query
                .OrderByDescending(p => p.FechaPostulacion)
                .ThenByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<bool> UpdateEstadoAsync(int id, EstadoPostulacion estado)
        {
            var existing = await context.Postulaciones.FindAsync(id);
            if (existing == null)
                return false;

            existing.SetEstado(estado);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await context.Postulaciones.FindAsync(id);
            if (existing == null)
                return false;

            context.Postulaciones.Remove(existing);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int alumnoId, int ofertaId)
        {
            return await context.Postulaciones.AnyAsync(p => p.AlumnoId == alumnoId && p.OfertaId == ofertaId);
        }
    }
}
