using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Services.Interfaces;

namespace SistemaGym.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public UsuarioService(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _unitOfWork.UsuarioRepository.GetAll();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _unitOfWork.UsuarioRepository.GetById(id);
        }

        public async Task InsertUsuario(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new Exception("El nombre del usuario es obligatorio");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new Exception("El email del usuario es obligatorio");

            if (string.IsNullOrWhiteSpace(usuario.Password))
                throw new Exception("La contraseña es obligatoria");

            usuario.Password = _passwordService.Hash(usuario.Password);
            usuario.FechaRegistro ??= DateTime.Now;
            usuario.Estado ??= true;

            await _unitOfWork.UsuarioRepository.Add(usuario);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUsuario(Usuario usuario)
        {
            if (!string.IsNullOrWhiteSpace(usuario.Password) &&
                !usuario.Password.Contains('.'))
            {
                usuario.Password = _passwordService.Hash(usuario.Password);
            }

            _unitOfWork.UsuarioRepository.Update(usuario);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUsuario(int id)
        {
            await _unitOfWork.UsuarioRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
