using FluentValidation;

namespace Application.Features.KatalogKonulari.Commands.Delete;

public class DeleteKatalogKonuCommandValidator : AbstractValidator<DeleteKatalogKonuCommand>
{
    public DeleteKatalogKonuCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
