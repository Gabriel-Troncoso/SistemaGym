using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class CrearMembresiaDtoValidator : AbstractValidator<MembresiaDto>
    {
        public CrearMembresiaDtoValidator()
        {
            // Para creación, el ID no debería enviarse o debería ser 0
            RuleFor(x => x.Id)
                .Equal(0).When(x => x.Id != 0)
                .WithMessage("El ID debe ser 0 o no enviarse para crear una nueva membresía.");

            // Validación para ClienteId
            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("El ID del cliente es obligatorio y debe ser mayor que cero.");

            // Validación para PlanMembresiaId
            RuleFor(x => x.PlanMembresiaId)
                .GreaterThan(0).WithMessage("El ID del plan de membresía es obligatorio y debe ser mayor que cero.");

            // Validación para FechaInicio
            RuleFor(x => x.FechaInicio)
                .NotNull().WithMessage("La fecha de inicio es obligatoria.");

            // Validación para FechaFin
            RuleFor(x => x.FechaFin)
                .NotNull().WithMessage("La fecha fin es obligatoria.");

            // Validación para rango de fechas
            RuleFor(x => x)
                .Must(HaveValidDateRange)
                .WithMessage("La fecha fin no puede ser menor que la fecha inicio.");

            // Validación para Estado
            RuleFor(x => x.Estado)
                .NotNull().WithMessage("El estado de la membresía es obligatorio.");
        }

        private bool HaveValidDateRange(MembresiaDto membresia)
        {
            if (membresia.FechaInicio.HasValue && membresia.FechaFin.HasValue)
            {
                return membresia.FechaFin.Value >= membresia.FechaInicio.Value;
            }

            return true;
        }
    }
}