using FluentValidation;

namespace Application.Features.MateryalEtiketleri.Commands.Delete;

public class DeleteMateryalEtiketCommandValidator : AbstractValidator<DeleteMateryalEtiketCommand>
{
    public DeleteMateryalEtiketCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
