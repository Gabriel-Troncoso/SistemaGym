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
            CreateMap<Usuario, SecurityDto>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Role, opt => opt.Ignore());
            CreateMap<SecurityDto, Usuario>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(_ => DateTime.Now));
        }
    }
}
