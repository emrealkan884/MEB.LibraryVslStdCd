using FluentValidation;

namespace Application.Features.OduncIslemler.Commands.Create;

public class CreateOduncIslemiCommandValidator : AbstractValidator<CreateOduncIslemiCommand>
{
    public CreateOduncIslemiCommandValidator()
    {
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.KullaniciId).NotEmpty();
        RuleFor(c => c.NushaId).NotEmpty();
    }
}
