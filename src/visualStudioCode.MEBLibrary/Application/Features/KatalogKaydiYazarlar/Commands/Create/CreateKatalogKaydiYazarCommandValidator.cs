using FluentValidation;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Create;

public class CreateKatalogKaydiYazarCommandValidator : AbstractValidator<CreateKatalogKaydiYazarCommand>
{
    public CreateKatalogKaydiYazarCommandValidator()
    {
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
        RuleFor(c => c.YazarId).NotEmpty();
    }
}
