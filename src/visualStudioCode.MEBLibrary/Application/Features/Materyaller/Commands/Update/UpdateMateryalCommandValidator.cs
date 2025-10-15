using FluentValidation;

namespace Application.Features.Materyaller.Commands.Update;

public class UpdateMateryalCommandValidator : AbstractValidator<UpdateMateryalCommand>
{
    public UpdateMateryalCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KatalogKaydiId).NotEmpty();
        RuleFor(c => c.KutuphaneId).NotEmpty();
    }
}
