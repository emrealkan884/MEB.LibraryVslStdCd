using FluentValidation;

namespace Application.Features.Kutuphaneler.Commands.Delete;

public class DeleteKutuphaneCommandValidator : AbstractValidator<DeleteKutuphaneCommand>
{
    public DeleteKutuphaneCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
