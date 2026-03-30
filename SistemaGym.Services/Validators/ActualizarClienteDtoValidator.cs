using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class ActualizarClienteDtoValidator : AbstractValidator<ClienteDto>
    {
        public ActualizarClienteDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del cliente es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100);

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(100);

            RuleFor(x => x.Ci)
                .NotEmpty().WithMessage("El CI es obligatorio.")
                .MaximumLength(20);

            RuleFor(x => x.Telefono)
                .MaximumLength(20);

            RuleFor(x => x.Correo)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Correo))
                .WithMessage("El correo no es válido.");
        }
    }
}