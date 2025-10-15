using FluentValidation;

namespace Application.Features.MateryalFormatDetaylari.Commands.Delete;

public class DeleteMateryalFormatDetayCommandValidator : AbstractValidator<DeleteMateryalFormatDetayCommand>
{
    public DeleteMateryalFormatDetayCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
