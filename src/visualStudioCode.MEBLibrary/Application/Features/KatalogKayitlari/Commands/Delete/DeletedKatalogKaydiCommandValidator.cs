using FluentValidation;

namespace Application.Features.KatalogKayitlari.Commands.Delete;

public class DeleteKatalogKaydiCommandValidator : AbstractValidator<DeleteKatalogKaydiCommand>
{
    public DeleteKatalogKaydiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
