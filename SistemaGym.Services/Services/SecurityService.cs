using SistemaGym.Core.Entities;
using SistemaGym.Core.Interfaces;
using SistemaGym.Services.Interfaces;

namespace SistemaGym.Services.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Usuario> GetLoginByCredentials(UserLogin userLogin)
        {
            return await _unitOfWork.UsuarioRepository.GetLoginByCredentials(userLogin);
        }

        public async Task RegisterUser(Usuario usuario)
        {
            await _unitOfWork.UsuarioRepository.Add(usuario);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
