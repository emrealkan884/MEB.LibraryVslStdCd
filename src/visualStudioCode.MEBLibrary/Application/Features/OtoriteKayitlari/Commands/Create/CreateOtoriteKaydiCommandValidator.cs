using FluentValidation;

namespace Application.Features.OtoriteKayitlari.Commands.Create;

public class CreateOtoriteKaydiCommandValidator : AbstractValidator<CreateOtoriteKaydiCommand>
{
    public CreateOtoriteKaydiCommandValidator()
    {
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
