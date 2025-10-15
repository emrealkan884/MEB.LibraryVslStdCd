using FluentValidation;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Update;

public class UpdateKatalogKaydiYazarCommandValidator : AbstractValidator<UpdateKatalogKaydiYazarCommand>
{
    public UpdateKatalogKaydiYazarCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
        RuleFor(c => c.YazarId).NotEmpty();
    }
}
