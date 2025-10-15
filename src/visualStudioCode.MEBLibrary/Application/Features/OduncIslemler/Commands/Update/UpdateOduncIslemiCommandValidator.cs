using FluentValidation;

namespace Application.Features.OduncIslemler.Commands.Update;

public class UpdateOduncIslemiCommandValidator : AbstractValidator<UpdateOduncIslemiCommand>
{
    public UpdateOduncIslemiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.KullaniciId).NotEmpty();
        RuleFor(c => c.NushaId).NotEmpty();
    }
}
