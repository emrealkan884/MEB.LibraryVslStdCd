using FluentValidation;

namespace Application.Features.Rezervasyonlar.Commands.Create;

public class CreateRezervasyonCommandValidator : AbstractValidator<CreateRezervasyonCommand>
{
    public CreateRezervasyonCommandValidator()
    {
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.KullaniciId).NotEmpty();
        RuleFor(c => c.MateryalId).NotEmpty();
    }
}
