using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CarreraRepository : ICarreraRepository
    {
        private readonly BolsaTrabajoContext context;

        public CarreraRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Carrera>> GetAllAsync()
        {
            return await context.Carreras.OrderBy(c => c.NomCarrera).ToListAsync();
        }

        public async Task<Carrera?> GetAsync(int id)
        {
            return await context.Carreras.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
