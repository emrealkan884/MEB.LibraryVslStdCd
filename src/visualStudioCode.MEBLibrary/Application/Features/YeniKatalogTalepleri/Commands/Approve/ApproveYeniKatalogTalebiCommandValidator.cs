using FluentValidation;

namespace Application.Features.YeniKatalogTalepleri.Commands.Approve;

public class ApproveYeniKatalogTalebiCommandValidator : AbstractValidator<ApproveYeniKatalogTalebiCommand>
{
    public ApproveYeniKatalogTalebiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
