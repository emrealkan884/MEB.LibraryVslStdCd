using FluentValidation;

namespace Application.Features.KatalogKayitlari.Commands.Create;

public class CreateKatalogKaydiCommandValidator : AbstractValidator<CreateKatalogKaydiCommand>
{
    public CreateKatalogKaydiCommandValidator()
    {
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.Baslik).NotEmpty();
    }
}
