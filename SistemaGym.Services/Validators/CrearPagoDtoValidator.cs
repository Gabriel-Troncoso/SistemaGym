using FluentValidation;
using SistemaGym.Core.DTOs;

namespace SistemaGym.Services.Validators
{
    public class CrearPagoDtoValidator : AbstractValidator<PagoDto>
    {
        public CrearPagoDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).When(x => x.Id != 0);

            RuleFor(x => x.MembresiaId).GreaterThan(0);
            RuleFor(x => x.Monto).GreaterThan(0);
            RuleFor(x => x.MetodoPago).NotEmpty();
        }
    }
}