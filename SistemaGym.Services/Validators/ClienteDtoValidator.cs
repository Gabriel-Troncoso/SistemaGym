using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class ClienteDtoValidator : AbstractValidator<ClienteDto>
    {
        public ClienteDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres.");

            RuleFor(x => x.Ci)
                .NotEmpty().WithMessage("El CI es obligatorio.")
                .MaximumLength(20).WithMessage("El CI no puede exceder 20 caracteres.");

            RuleFor(x => x.Telefono)
                .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres.");

            RuleFor(x => x.Correo)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Correo))
                .WithMessage("El correo no es válido.");
                
            RuleFor(x => x.FechaRegistro)
                .Must(BeNotFutureDate).When(x => x.FechaRegistro.HasValue)
                .WithMessage("La fecha no puede ser futura.");
        }

        private bool BeNotFutureDate(DateTime? date)
        {
            if (date.HasValue)
                return date.Value <= DateTime.Now;
            return true;
        }
    }
}