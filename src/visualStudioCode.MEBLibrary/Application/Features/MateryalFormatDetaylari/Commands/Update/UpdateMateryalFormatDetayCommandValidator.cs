using FluentValidation;

namespace Application.Features.MateryalFormatDetaylari.Commands.Update;

public class UpdateMateryalFormatDetayCommandValidator : AbstractValidator<UpdateMateryalFormatDetayCommand>
{
    public UpdateMateryalFormatDetayCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
    }
}
