using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class PlanMembresiaDtoValidator : AbstractValidator<PlanMembresiaDto>
    {
        public PlanMembresiaDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);

            RuleFor(x => x.NombrePlan)
                .NotEmpty().WithMessage("El nombre del plan es obligatorio")
                .MaximumLength(100);

            RuleFor(x => x.Descripcion)
                .MaximumLength(300);

            RuleFor(x => x.DuracionDias)
                .GreaterThan(0).When(x => x.DuracionDias.HasValue)
                .WithMessage("La duración debe ser mayor que cero");

            RuleFor(x => x.Precio)
                .GreaterThan(0).When(x => x.Precio.HasValue)
                .WithMessage("El precio debe ser mayor que cero");
        }
    }
}