using FluentValidation;

namespace Application.Features.MateryalEtiketleri.Commands.Create;

public class CreateMateryalEtiketCommandValidator : AbstractValidator<CreateMateryalEtiketCommand>
{
    public CreateMateryalEtiketCommandValidator()
    {
        RuleFor(c => c.MateryalId).NotEmpty();
        RuleFor(c => c.Etiket).NotEmpty();
    }
}
