using FluentValidation;

namespace Application.Features.OtoriteKayitlari.Commands.Create;

public class CreateOtoriteKaydiCommandValidator : AbstractValidator<CreateOtoriteKaydiCommand>
{
    public CreateOtoriteKaydiCommandValidator()
    {
        RuleFor(c => c.YetkiliBaslik).NotEmpty();
    }
}
