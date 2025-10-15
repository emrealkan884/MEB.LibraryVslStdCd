using FluentValidation;

namespace Application.Features.Etkinlikler.Commands.Create;

public class CreateEtkinlikCommandValidator : AbstractValidator<CreateEtkinlikCommand>
{
    public CreateEtkinlikCommandValidator()
    {
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.Baslik).NotEmpty();
    }
}
