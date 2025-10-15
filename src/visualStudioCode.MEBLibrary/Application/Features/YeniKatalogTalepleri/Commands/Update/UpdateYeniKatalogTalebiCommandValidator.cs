using FluentValidation;

namespace Application.Features.YeniKatalogTalepleri.Commands.Update;

public class UpdateYeniKatalogTalebiCommandValidator : AbstractValidator<UpdateYeniKatalogTalebiCommand>
{
    public UpdateYeniKatalogTalebiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.TalepEdenKutuphaneId).NotEmpty();
        RuleFor(c => c.Baslik).NotEmpty();
    }
}
