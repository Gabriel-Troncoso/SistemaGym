using SistemaGym.Core.CustomEntities;
using SistemaGym.Core.Entities;
using SistemaGym.Core.QueryFilters;

namespace SistemaGym.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync(UsuarioQueryFilter? filters = null);

        Task<ResponseData> GetAllUsuariosResponseAsync(UsuarioQueryFilter? filters = null);

        Task<Usuario> GetUsuarioByIdAsync(int id);

        Task InsertUsuario(Usuario usuario);

        Task UpdateUsuario(Usuario usuario);

        Task DeleteUsuario(int id);
    }
}
