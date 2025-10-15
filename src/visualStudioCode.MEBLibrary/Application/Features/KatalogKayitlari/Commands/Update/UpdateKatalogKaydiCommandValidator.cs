using FluentValidation;

namespace Application.Features.KatalogKayitlari.Commands.Update;

public class UpdateKatalogKaydiCommandValidator : AbstractValidator<UpdateKatalogKaydiCommand>
{
    public UpdateKatalogKaydiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.Baslik).NotEmpty();
    }
}
