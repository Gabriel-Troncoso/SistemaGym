using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class ActualizarPlanMembresiaDtoValidator : AbstractValidator<PlanMembresiaDto>
    {
        public ActualizarPlanMembresiaDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID es obligatorio para actualizar.");

            RuleFor(x => x.NombrePlan)
                .NotEmpty().WithMessage("El nombre del plan es obligatorio.");

            RuleFor(x => x.DuracionDias)
                .GreaterThan(0).WithMessage("La duración debe ser mayor a 0.");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
        }
    }
}