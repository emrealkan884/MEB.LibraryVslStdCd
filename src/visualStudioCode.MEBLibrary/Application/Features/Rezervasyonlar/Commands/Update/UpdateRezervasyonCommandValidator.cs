using FluentValidation;

namespace Application.Features.Rezervasyonlar.Commands.Update;

public class UpdateRezervasyonCommandValidator : AbstractValidator<UpdateRezervasyonCommand>
{
    public UpdateRezervasyonCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.KullaniciId).NotEmpty();
        RuleFor(c => c.MateryalId).NotEmpty();
    }
}
