using FluentValidation;

namespace Application.Features.Materyaller.Commands.Delete;

public class DeleteMateryalCommandValidator : AbstractValidator<DeleteMateryalCommand>
{
    public DeleteMateryalCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
