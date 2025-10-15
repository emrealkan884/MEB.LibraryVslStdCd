using FluentValidation;

namespace Application.Features.OtoriteKayitlari.Commands.Update;

public class UpdateOtoriteKaydiCommandValidator : AbstractValidator<UpdateOtoriteKaydiCommand>
{
    public UpdateOtoriteKaydiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.YetkiliBaslik)
            .NotEmpty()
            .MinimumLength(3);
        RuleFor(c => c.OtoriteTuru)
            .IsInEnum();
        RuleFor(c => c.HariciKayitNo)
            .MaximumLength(100);
        RuleFor(c => c.AlternatifBasliklar)
            .MaximumLength(500);
        RuleFor(c => c.BagliTerimler)
            .MaximumLength(500);
    }
}
