using FluentValidation;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Create;

public class CreateKatalogKaydiYazarCommandValidator : AbstractValidator<CreateKatalogKaydiYazarCommand>
{
    public CreateKatalogKaydiYazarCommandValidator()
    {
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
        RuleFor(c => c.YazarId).NotEmpty();
        RuleFor(c => c.OtoriteKaydiId).NotEmpty();
        RuleFor(c => c.Sira).GreaterThanOrEqualTo(1);
    }
}
