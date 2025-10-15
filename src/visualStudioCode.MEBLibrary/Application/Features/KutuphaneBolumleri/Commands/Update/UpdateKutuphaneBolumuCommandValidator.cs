using FluentValidation;

namespace Application.Features.KutuphaneBolumleri.Commands.Update;

public class UpdateKutuphaneBolumuCommandValidator : AbstractValidator<UpdateKutuphaneBolumuCommand>
{
    public UpdateKutuphaneBolumuCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.KutuphaneId).NotEmpty();
        RuleFor(c => c.Ad).NotEmpty();
    }
}
