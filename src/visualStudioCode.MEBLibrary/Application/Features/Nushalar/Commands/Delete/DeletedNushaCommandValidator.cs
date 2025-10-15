using FluentValidation;

namespace Application.Features.Nushalar.Commands.Delete;

public class DeleteNushaCommandValidator : AbstractValidator<DeleteNushaCommand>
{
    public DeleteNushaCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
