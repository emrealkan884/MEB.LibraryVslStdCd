using FluentValidation;

namespace Application.Features.OduncIslemler.Commands.Delete;

public class DeleteOduncIslemiCommandValidator : AbstractValidator<DeleteOduncIslemiCommand>
{
    public DeleteOduncIslemiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
