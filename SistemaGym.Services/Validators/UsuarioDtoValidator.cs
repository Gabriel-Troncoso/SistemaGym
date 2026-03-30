using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class UsuarioDtoValidator : AbstractValidator<UsuarioDto>
    {
        public UsuarioDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100);

            RuleFor(x => x.Telefono)
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El email no es válido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(4).WithMessage("La contraseña debe tener al menos 4 caracteres");

            RuleFor(x => x.Rol)
                .NotEmpty().WithMessage("El rol es obligatorio")
                .MaximumLength(30);

            RuleFor(x => x.FechaRegistro)
                .LessThanOrEqualTo(DateTime.Now)
                .When(x => x.FechaRegistro.HasValue);
        }
    }
}