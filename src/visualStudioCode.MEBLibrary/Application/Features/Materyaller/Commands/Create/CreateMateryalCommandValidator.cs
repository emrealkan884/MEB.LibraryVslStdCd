using FluentValidation;

namespace Application.Features.Materyaller.Commands.Create;

public class CreateMateryalCommandValidator : AbstractValidator<CreateMateryalCommand>
{
    public CreateMateryalCommandValidator()
    {
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
        RuleFor(c => c.KutuphaneId).NotEmpty();
    }
}
