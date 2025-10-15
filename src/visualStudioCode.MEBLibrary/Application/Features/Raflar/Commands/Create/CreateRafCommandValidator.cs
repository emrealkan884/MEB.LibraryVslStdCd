using FluentValidation;

namespace Application.Features.Raflar.Commands.Create;

public class CreateRafCommandValidator : AbstractValidator<CreateRafCommand>
{
    public CreateRafCommandValidator()
    {
        RuleFor(c => c.Kod).NotEmpty();
    }
}
