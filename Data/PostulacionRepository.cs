// Guía de estudio: Acceso a datos de Postulacion mediante Entity Framework Core: construye consultas y persiste cambios usando BolsaTrabajoContext. Separa los detalles de base de datos de los servicios.
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class PostulacionRepository : IPostulacionRepository
    {
        private readonly BolsaTrabajoContext context;

        // Inicializa el objeto y sus dependencias para las operaciones de esta clase.
        public PostulacionRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        // Prepara la consulta incluyendo alumno, carrera, oferta y empresa para completar los datos de las postulaciones.
        private IQueryable<Postulacion> ConDetalle()
        {
            return context.Postulaciones
                .Include(p => p.Alumno).ThenInclude(a => a!.Carrera)
                .Include(p => p.Oferta).ThenInclude(o => o!.Empresa);
        }

        // Agrega un registro en la base de datos mediante el contexto de EF Core.
        public async Task AddAsync(Postulacion postulacion)
        {
            context.Postulaciones.Add(postulacion);
            await context.SaveChangesAsync();
        }

        // Busca un registro por su identificador en la base de datos mediante el contexto de EF Core.
        public async Task<Postulacion?> GetAsync(int id)
        {
            return await ConDetalle().FirstOrDefaultAsync(p => p.Id == id);
        }

        // Consulta la colección de registros, respetando los filtros opcionales recibidos.
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

        // Actualiza el registro o su estado en la base de datos mediante el contexto de EF Core.
        public async Task<bool> UpdateEstadoAsync(int id, EstadoPostulacion estado)
        {
            var existing = await context.Postulaciones.FindAsync(id);
            if (existing == null)
                return false;

            existing.SetEstado(estado);
            await context.SaveChangesAsync();
            return true;
        }

        // Elimina el registro indicado en la base de datos mediante el contexto de EF Core.
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await context.Postulaciones.FindAsync(id);
            if (existing == null)
                return false;

            context.Postulaciones.Remove(existing);
            await context.SaveChangesAsync();
            return true;
        }

        // Comprueba si ya existe un registro coincidente; se utiliza para controlar duplicados.
        public async Task<bool> ExistsAsync(int alumnoId, int ofertaId)
        {
            return await context.Postulaciones.AnyAsync(p => p.AlumnoId == alumnoId && p.OfertaId == ofertaId);
        }
    }
}
