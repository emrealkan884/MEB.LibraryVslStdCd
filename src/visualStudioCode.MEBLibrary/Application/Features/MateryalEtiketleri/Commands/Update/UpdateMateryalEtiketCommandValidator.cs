using FluentValidation;

namespace Application.Features.MateryalEtiketleri.Commands.Update;

public class UpdateMateryalEtiketCommandValidator : AbstractValidator<UpdateMateryalEtiketCommand>
{
    public UpdateMateryalEtiketCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.MateryalId).NotEmpty();
        RuleFor(c => c.Etiket).NotEmpty();
    }
}
