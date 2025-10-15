using FluentValidation;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Delete;

public class DeleteKatalogKaydiYazarCommandValidator : AbstractValidator<DeleteKatalogKaydiYazarCommand>
{
    public DeleteKatalogKaydiYazarCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
