using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class CrearPlanMembresiaDtoValidator : AbstractValidator<PlanMembresiaDto>
    {
        public CrearPlanMembresiaDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).When(x => x.Id != 0)
                .WithMessage("El ID debe ser 0 al crear un plan.");

            RuleFor(x => x.NombrePlan)
                .NotEmpty().WithMessage("El nombre del plan es obligatorio.");

            RuleFor(x => x.DuracionDias)
                .GreaterThan(0).WithMessage("La duración debe ser mayor a 0.");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
        }
    }
}