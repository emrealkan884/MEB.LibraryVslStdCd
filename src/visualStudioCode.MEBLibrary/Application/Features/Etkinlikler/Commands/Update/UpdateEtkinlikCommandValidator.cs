using FluentValidation;

namespace Application.Features.Etkinlikler.Commands.Update;

public class UpdateEtkinlikCommandValidator : AbstractValidator<UpdateEtkinlikCommand>
{
    public UpdateEtkinlikCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.Baslik).NotEmpty();
    }
}
