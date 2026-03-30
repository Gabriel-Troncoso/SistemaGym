using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class CrearClienteDtoValidator : AbstractValidator<ClienteDto>
    {
        public CrearClienteDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).When(x => x.Id != 0)
                .WithMessage("El ID debe ser 0 o no enviarse para crear un nuevo cliente.");

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

            RuleFor(x => x.FechaRegistro)
                .LessThanOrEqualTo(DateTime.Now)
                .When(x => x.FechaRegistro.HasValue)
                .WithMessage("La fecha no puede ser futura.");
        }
    }
}