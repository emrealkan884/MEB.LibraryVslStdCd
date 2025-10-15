using FluentValidation;

namespace Application.Features.KatalogKonulari.Commands.Update;

public class UpdateKatalogKonuCommandValidator : AbstractValidator<UpdateKatalogKonuCommand>
{
    public UpdateKatalogKonuCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
        RuleFor(c => c.KonuBasligi).NotEmpty();
        RuleFor(c => c.OtoriteKaydiId).NotEmpty();
    }
}
