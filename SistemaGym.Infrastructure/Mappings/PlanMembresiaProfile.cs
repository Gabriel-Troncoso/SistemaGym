using AutoMapper;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Mappings
{
    public class PlanMembresiaProfile : Profile
    {
        public PlanMembresiaProfile()
        {
            CreateMap<PlanMembresia, PlanMembresiaDto>();
            CreateMap<PlanMembresiaDto, PlanMembresia>();
        }
    }
}