using FluentValidation;

namespace Application.Features.DeweySiniflamalari.Commands.Update;

public class UpdateDeweySiniflamaCommandValidator : AbstractValidator<UpdateDeweySiniflamaCommand>
{
    public UpdateDeweySiniflamaCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Kod).NotEmpty();
        RuleFor(c => c.Baslik).NotEmpty();
    }
}
