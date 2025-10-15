using FluentValidation;

namespace Application.Features.YeniKatalogTalepleri.Commands.Delete;

public class DeleteYeniKatalogTalebiCommandValidator : AbstractValidator<DeleteYeniKatalogTalebiCommand>
{
    public DeleteYeniKatalogTalebiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
