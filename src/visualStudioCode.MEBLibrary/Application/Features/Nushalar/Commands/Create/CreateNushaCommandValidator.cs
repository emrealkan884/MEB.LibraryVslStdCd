using FluentValidation;

namespace Application.Features.Nushalar.Commands.Create;

public class CreateNushaCommandValidator : AbstractValidator<CreateNushaCommand>
{
    public CreateNushaCommandValidator()
    {
        RuleFor(c => c.MateryalId).NotEmpty();
        RuleFor(c => c.Barkod).NotEmpty();
    }
}
