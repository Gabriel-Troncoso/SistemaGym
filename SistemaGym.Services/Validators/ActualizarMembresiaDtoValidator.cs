using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class ActualizarMembresiaDtoValidator : AbstractValidator<MembresiaDto>
    {
        public ActualizarMembresiaDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.PlanMembresiaId).GreaterThan(0);

            RuleFor(x => x)
                .Must(x => !x.FechaInicio.HasValue || !x.FechaFin.HasValue || x.FechaFin >= x.FechaInicio)
                .WithMessage("La fecha fin no puede ser menor a la fecha inicio");
        }
    }
}