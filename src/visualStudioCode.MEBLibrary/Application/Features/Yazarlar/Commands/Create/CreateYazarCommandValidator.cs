using FluentValidation;

namespace Application.Features.Yazarlar.Commands.Create;

public class CreateYazarCommandValidator : AbstractValidator<CreateYazarCommand>
{
    public CreateYazarCommandValidator()
    {
        RuleFor(c => c.AdSoyad).NotEmpty();
    }
}