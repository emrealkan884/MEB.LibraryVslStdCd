using FluentValidation;

namespace Application.Features.KatalogKonulari.Commands.Create;

public class CreateKatalogKonuCommandValidator : AbstractValidator<CreateKatalogKonuCommand>
{
    public CreateKatalogKonuCommandValidator()
    {
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
        RuleFor(c => c.KonuBasligi).NotEmpty();
    }
}
