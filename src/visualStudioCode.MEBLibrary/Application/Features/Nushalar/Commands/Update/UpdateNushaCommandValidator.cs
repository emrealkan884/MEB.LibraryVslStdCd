using FluentValidation;

namespace Application.Features.Nushalar.Commands.Update;

public class UpdateNushaCommandValidator : AbstractValidator<UpdateNushaCommand>
{
    public UpdateNushaCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.MateryalId).NotEmpty();
        RuleFor(c => c.Barkod).NotEmpty();
    }
}
