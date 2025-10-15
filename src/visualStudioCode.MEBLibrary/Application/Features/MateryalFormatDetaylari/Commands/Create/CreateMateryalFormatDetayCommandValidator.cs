using FluentValidation;

namespace Application.Features.MateryalFormatDetaylari.Commands.Create;

public class CreateMateryalFormatDetayCommandValidator : AbstractValidator<CreateMateryalFormatDetayCommand>
{
    public CreateMateryalFormatDetayCommandValidator()
    {
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
    }
}
