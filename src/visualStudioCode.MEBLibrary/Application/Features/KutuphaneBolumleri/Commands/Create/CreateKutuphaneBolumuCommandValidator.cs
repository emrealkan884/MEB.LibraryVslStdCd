using FluentValidation;

namespace Application.Features.KutuphaneBolumleri.Commands.Create;

public class CreateKutuphaneBolumuCommandValidator : AbstractValidator<CreateKutuphaneBolumuCommand>
{
    public CreateKutuphaneBolumuCommandValidator()
    {
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.Ad).NotEmpty();
    }
}
