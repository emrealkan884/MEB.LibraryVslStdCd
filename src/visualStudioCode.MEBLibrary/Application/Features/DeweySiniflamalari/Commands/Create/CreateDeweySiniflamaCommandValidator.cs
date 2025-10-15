using FluentValidation;

namespace Application.Features.DeweySiniflamalari.Commands.Create;

public class CreateDeweySiniflamaCommandValidator : AbstractValidator<CreateDeweySiniflamaCommand>
{
    public CreateDeweySiniflamaCommandValidator()
    {
        RuleFor(c => c.Kod).NotEmpty();
        RuleFor(c => c.Baslik).NotEmpty();
    }
}
