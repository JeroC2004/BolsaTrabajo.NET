using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByUsernameAsync(string username);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BolsaTrabajoContext context;

        public UsuarioRepository(BolsaTrabajoContext context)
        {
            this.context = context;
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
        {
            return await context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Activo);
        }
    }
}
