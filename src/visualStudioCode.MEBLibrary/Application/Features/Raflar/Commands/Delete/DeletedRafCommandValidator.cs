using FluentValidation;

namespace Application.Features.Raflar.Commands.Delete;

public class DeleteRafCommandValidator : AbstractValidator<DeleteRafCommand>
{
    public DeleteRafCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
