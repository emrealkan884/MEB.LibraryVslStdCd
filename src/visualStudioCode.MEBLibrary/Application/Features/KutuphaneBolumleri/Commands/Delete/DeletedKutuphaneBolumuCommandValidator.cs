using FluentValidation;

namespace Application.Features.KutuphaneBolumleri.Commands.Delete;

public class DeleteKutuphaneBolumuCommandValidator : AbstractValidator<DeleteKutuphaneBolumuCommand>
{
    public DeleteKutuphaneBolumuCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
