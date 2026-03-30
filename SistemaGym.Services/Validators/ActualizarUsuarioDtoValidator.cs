using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class ActualizarUsuarioDtoValidator : AbstractValidator<UsuarioDto>
    {
        public ActualizarUsuarioDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID es obligatorio para actualizar.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no es válido.");

            RuleFor(x => x.Rol)
                .NotEmpty().WithMessage("El rol es obligatorio.");
        }
    }
}