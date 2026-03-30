using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Services.Interfaces;

namespace SistemaGym.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IBaseRepository<Usuario> _usuarioRepository;

        public UsuarioService(IBaseRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _usuarioRepository.GetAll();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _usuarioRepository.GetById(id);
        }

        public async Task InsertUsuario(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new Exception("El nombre del usuario es obligatorio");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new Exception("El email del usuario es obligatorio");

            if (string.IsNullOrWhiteSpace(usuario.Password))
                throw new Exception("La contraseña es obligatoria");

            await _usuarioRepository.Add(usuario);
        }

        public async Task UpdateUsuario(Usuario usuario)
        {
            await _usuarioRepository.Update(usuario);
        }

        public async Task DeleteUsuario(int id)
        {
            await _usuarioRepository.Delete(id);
        }
    }
}