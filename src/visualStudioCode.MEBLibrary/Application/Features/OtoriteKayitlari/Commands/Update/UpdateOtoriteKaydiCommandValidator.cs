using FluentValidation;

namespace Application.Features.OtoriteKayitlari.Commands.Update;

public class UpdateOtoriteKaydiCommandValidator : AbstractValidator<UpdateOtoriteKaydiCommand>
{
    public UpdateOtoriteKaydiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.YetkiliBaslik).NotEmpty();
    }
}
