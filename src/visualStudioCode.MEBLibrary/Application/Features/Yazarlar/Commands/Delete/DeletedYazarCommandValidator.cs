using FluentValidation;

namespace Application.Features.Yazarlar.Commands.Delete;

public class DeleteYazarCommandValidator : AbstractValidator<DeleteYazarCommand>
{
    public DeleteYazarCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}