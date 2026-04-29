using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class CrearPlanMembresiaDtoValidator : AbstractValidator<PlanMembresiaDto>
    {
        public CrearPlanMembresiaDtoValidator()
        {
            // Para creación, el ID no debería enviarse o debería ser 0
            RuleFor(x => x.Id)
                .Equal(0).When(x => x.Id != 0)
                .WithMessage("El ID debe ser 0 o no enviarse para crear un nuevo plan.");

            // Validación para NombrePlan
            RuleFor(x => x.NombrePlan)
                .NotEmpty().WithMessage("El nombre del plan es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre del plan debe tener al menos 3 caracteres.")
                .MaximumLength(100).WithMessage("El nombre del plan no puede exceder los 100 caracteres.");

            // Validación para Descripción
            RuleFor(x => x.Descripcion)
                .MaximumLength(300).WithMessage("La descripción no puede exceder los 300 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Descripcion));

            // Validación para DuracionDias
            RuleFor(x => x.DuracionDias)
                .NotNull().WithMessage("La duración del plan es obligatoria.")
                .GreaterThan(0).WithMessage("La duración debe ser mayor que cero.");

            // Validación para Precio
            RuleFor(x => x.Precio)
                .NotNull().WithMessage("El precio del plan es obligatorio.")
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");
        }
    }
}