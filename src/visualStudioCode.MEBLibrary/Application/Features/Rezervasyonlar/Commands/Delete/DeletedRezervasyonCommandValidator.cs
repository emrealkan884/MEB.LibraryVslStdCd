using FluentValidation;

namespace Application.Features.Rezervasyonlar.Commands.Delete;

public class DeleteRezervasyonCommandValidator : AbstractValidator<DeleteRezervasyonCommand>
{
    public DeleteRezervasyonCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
