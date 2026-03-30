using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class PagoDtoValidator : AbstractValidator<PagoDto>
    {
        public PagoDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);

            RuleFor(x => x.MembresiaId)
                .GreaterThan(0).WithMessage("La membresía es obligatoria");

            RuleFor(x => x.Monto)
                .GreaterThan(0).When(x => x.Monto.HasValue)
                .WithMessage("El monto debe ser mayor a cero");

            RuleFor(x => x.MetodoPago)
                .NotEmpty().WithMessage("El método de pago es obligatorio")
                .MaximumLength(50);

            RuleFor(x => x.FechaPago)
                .LessThanOrEqualTo(DateTime.Now)
                .When(x => x.FechaPago.HasValue)
                .WithMessage("La fecha de pago no puede ser futura");
        }
    }
}