using FluentValidation;

namespace Application.Features.Kutuphaneler.Commands.Create;

public class CreateKutuphaneCommandValidator : AbstractValidator<CreateKutuphaneCommand>
{
    public CreateKutuphaneCommandValidator()
    {
        RuleFor(c => c.Kod).NotEmpty();
        RuleFor(c => c.Ad).NotEmpty();
        RuleFor(c => c.Adres).NotEmpty();
    }
}
