using SistemaGym.Core.Entities;

namespace SistemaGym.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<Usuario> GetLoginByCredentials(UserLogin userLogin);

        Task RegisterUser(Usuario usuario);
    }
}
