using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class ClienteDtoValidator : AbstractValidator<ClienteDto>
    {
        public ClienteDtoValidator()
        {
            // Validación para Nombre
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            // Validación para Apellido
            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MinimumLength(2).WithMessage("El apellido debe tener al menos 2 caracteres.")
                .MaximumLength(100).WithMessage("El apellido no puede exceder los 100 caracteres.");

            // Validación para CI
            RuleFor(x => x.Ci)
                .NotEmpty().WithMessage("El CI es obligatorio.")
                .MinimumLength(3).WithMessage("El CI debe tener al menos 3 caracteres.")
                .MaximumLength(20).WithMessage("El CI no puede exceder los 20 caracteres.");

            // Validación para Teléfono
            RuleFor(x => x.Telefono)
                .MaximumLength(20).WithMessage("El teléfono no puede exceder los 20 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Telefono));

            // Validación para Correo
            RuleFor(x => x.Correo)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Correo))
                .WithMessage("El correo no es válido.");

            // Validación para FechaRegistro
            RuleFor(x => x.FechaRegistro)
                .Must(BeNotFutureDate).When(x => x.FechaRegistro.HasValue)
                .WithMessage("La fecha de registro no puede ser futura.");
        }

        private bool BeNotFutureDate(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value <= DateTime.Now;
            }

            return true;
        }
    }
}