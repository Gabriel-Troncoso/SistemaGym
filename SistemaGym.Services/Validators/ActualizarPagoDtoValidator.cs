using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class ActualizarPagoDtoValidator : AbstractValidator<PagoDto>
    {
        public ActualizarPagoDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.MembresiaId).GreaterThan(0);
            RuleFor(x => x.Monto).GreaterThan(0);
            RuleFor(x => x.MetodoPago).NotEmpty();
        }
    }
}