using FluentValidation;

namespace Application.Features.Raflar.Commands.Update;

public class UpdateRafCommandValidator : AbstractValidator<UpdateRafCommand>
{
    public UpdateRafCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Kod).NotEmpty();
    }
}
