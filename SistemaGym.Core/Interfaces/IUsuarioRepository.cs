using SistemaGym.Core.Entities;

namespace SistemaGym.Core.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario> GetLoginByCredentials(UserLogin userLogin);
    }
}
