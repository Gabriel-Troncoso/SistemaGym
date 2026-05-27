using Microsoft.EntityFrameworkCore;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Infrastructure.Data;

namespace SistemaGym.Infrastructure.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SistemaGymContext context)
            : base(context)
        {
        }

        public async Task<Usuario> GetLoginByCredentials(UserLogin userLogin)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Email == userLogin.User);
        }
    }
}
