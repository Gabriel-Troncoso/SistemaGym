using SistemaGym.Core.Entities;
using SistemaGym.Core.CustomEntities;
using SistemaGym.Core.Enum;
using SistemaGym.Core.Interfaces;
using SistemaGym.Core.QueryFilters;
using SistemaGym.Services.Interfaces;
using System.Net;

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

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync(
            UsuarioQueryFilter? filters = null)
        {
            var usuarios = await _unitOfWork.UsuarioRepository.GetAll();

            if (filters != null)
            {
                if (!string.IsNullOrWhiteSpace(filters.Nombre))
                {
                    usuarios = usuarios.Where(u =>
                        u.Nombre != null &&
                        u.Nombre.ToLower().Contains(filters.Nombre.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filters.Telefono))
                {
                    usuarios = usuarios.Where(u =>
                        u.Telefono != null &&
                        u.Telefono.ToLower().Contains(filters.Telefono.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filters.Email))
                {
                    usuarios = usuarios.Where(u =>
                        u.Email != null &&
                        u.Email.ToLower().Contains(filters.Email.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(filters.Rol))
                {
                    usuarios = usuarios.Where(u =>
                        u.Rol != null &&
                        u.Rol.ToLower().Contains(filters.Rol.ToLower()));
                }

                if (filters.Estado != null)
                {
                    usuarios = usuarios.Where(u => u.Estado == filters.Estado);
                }
            }

            return usuarios;
        }

        public async Task<ResponseData> GetAllUsuariosResponseAsync(
            UsuarioQueryFilter? filters = null)
        {
            filters ??= new UsuarioQueryFilter();

            var usuarios = await GetAllUsuariosAsync(filters);
            var pagedUsuarios = PagedList<object>
                .Create(usuarios.Cast<object>(), filters.PageNumber, filters.PageSize);

            if (pagedUsuarios.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[]
                    {
                        new()
                        {
                            Type = TypeMessage.success.ToString(),
                            Description = "Registros de usuarios recuperados correctamente"
                        }
                    },
                    Pagination = pagedUsuarios,
                    StatusCode = HttpStatusCode.OK
                };
            }

            return new ResponseData
            {
                Messages = new Message[]
                {
                    new()
                    {
                        Type = TypeMessage.warning.ToString(),
                        Description = "No fue posible recuperar registros de usuarios"
                    }
                },
                Pagination = pagedUsuarios,
                StatusCode = HttpStatusCode.NotFound
            };
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
