using FluentValidation;

namespace Application.Features.YeniKatalogTalepleri.Commands.Create;

public class CreateYeniKatalogTalebiCommandValidator : AbstractValidator<CreateYeniKatalogTalebiCommand>
{
    public CreateYeniKatalogTalebiCommandValidator()
    {
        RuleFor(c => c.TalepEdenKutuphaneId).NotEmpty();
        RuleFor(c => c.Baslik).NotEmpty();
    }
}
