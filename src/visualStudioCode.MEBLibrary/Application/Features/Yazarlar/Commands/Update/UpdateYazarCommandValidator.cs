using FluentValidation;

namespace Application.Features.Yazarlar.Commands.Update;

public class UpdateYazarCommandValidator : AbstractValidator<UpdateYazarCommand>
{
    public UpdateYazarCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.AdSoyad).NotEmpty();
    }
}