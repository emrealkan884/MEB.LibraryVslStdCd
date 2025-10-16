using FluentValidation;

namespace Application.Features.YeniKatalogTalepleri.Commands.Review;

public class StartReviewYeniKatalogTalebiCommandValidator : AbstractValidator<StartReviewYeniKatalogTalebiCommand>
{
    public StartReviewYeniKatalogTalebiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
