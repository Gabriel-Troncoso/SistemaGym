using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class MembresiaDtoValidator : AbstractValidator<MembresiaDto>
    {
        public MembresiaDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);

            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("El ClienteId es obligatorio");

            RuleFor(x => x.PlanMembresiaId)
                .GreaterThan(0).WithMessage("El PlanMembresiaId es obligatorio");

            RuleFor(x => x.FechaInicio) 
                .NotNull().WithMessage("La fecha de inicio es obligatoria");

            RuleFor(x => x.FechaFin)
                .NotNull().WithMessage("La fecha fin es obligatoria");

            RuleFor(x => x)
                .Must(x => !x.FechaInicio.HasValue || !x.FechaFin.HasValue || x.FechaFin >= x.FechaInicio)
                .WithMessage("La fecha fin no puede ser menor a la fecha inicio");
        }
    }
}