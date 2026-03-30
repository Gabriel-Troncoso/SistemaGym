using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class CrearMembresiaDtoValidator : AbstractValidator<MembresiaDto>
    {
        public CrearMembresiaDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).When(x => x.Id != 0);

            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.PlanMembresiaId).GreaterThan(0);
            RuleFor(x => x.FechaInicio).NotNull();
            RuleFor(x => x.FechaFin).NotNull();

            RuleFor(x => x)
                .Must(x => !x.FechaInicio.HasValue || !x.FechaFin.HasValue || x.FechaFin >= x.FechaInicio)
                .WithMessage("La fecha fin no puede ser menor a la fecha inicio");
        }
    }
}