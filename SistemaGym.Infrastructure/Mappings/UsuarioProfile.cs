using AutoMapper;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<UsuarioDto, Usuario>();
        }
    }
}