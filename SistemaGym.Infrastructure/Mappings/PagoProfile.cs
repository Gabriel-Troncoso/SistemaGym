using AutoMapper;
using SistemaGym.Core.DTOs;
using SistemaGym.Core.Entities;

namespace SistemaGym.Infrastructure.Mappings
{
    public class PagoProfile : Profile
    {
        public PagoProfile()
        {
            CreateMap<Pago, PagoDto>();
            CreateMap<PagoDto, Pago>();
        }
    }
}