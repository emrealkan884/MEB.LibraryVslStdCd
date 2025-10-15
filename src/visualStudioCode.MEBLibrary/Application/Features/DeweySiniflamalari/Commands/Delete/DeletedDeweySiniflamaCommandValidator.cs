using FluentValidation;

namespace Application.Features.DeweySiniflamalari.Commands.Delete;

public class DeleteDeweySiniflamaCommandValidator : AbstractValidator<DeleteDeweySiniflamaCommand>
{
    public DeleteDeweySiniflamaCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
