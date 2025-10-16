using FluentValidation;

namespace Application.Features.YeniKatalogTalepleri.Commands.Reject;

public class RejectYeniKatalogTalebiCommandValidator : AbstractValidator<RejectYeniKatalogTalebiCommand>
{
    public RejectYeniKatalogTalebiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Gerekce).NotEmpty().MinimumLength(10);
    }
}
