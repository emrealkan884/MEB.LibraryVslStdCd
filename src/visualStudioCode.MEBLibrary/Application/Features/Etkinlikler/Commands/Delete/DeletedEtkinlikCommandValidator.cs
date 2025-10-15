using FluentValidation;

namespace Application.Features.Etkinlikler.Commands.Delete;

public class DeleteEtkinlikCommandValidator : AbstractValidator<DeleteEtkinlikCommand>
{
    public DeleteEtkinlikCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
