using FluentValidation;

namespace Application.Features.Kutuphaneler.Commands.Update;

public class UpdateKutuphaneCommandValidator : AbstractValidator<UpdateKutuphaneCommand>
{
    public UpdateKutuphaneCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Kod).NotEmpty();
        RuleFor(c => c.Ad).NotEmpty();
        RuleFor(c => c.Adres).NotEmpty();
    }
}
