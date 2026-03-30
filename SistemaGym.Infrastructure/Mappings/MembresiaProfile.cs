using AutoMapper;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Mappings
{
    public class MembresiaProfile : Profile
    {
        public MembresiaProfile()
        {
            CreateMap<Membresia, MembresiaDto>();
            CreateMap<MembresiaDto, Membresia>();
        }
    }
}