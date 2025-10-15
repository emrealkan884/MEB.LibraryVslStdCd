using FluentValidation;

namespace Application.Features.OtoriteKayitlari.Commands.Delete;

public class DeleteOtoriteKaydiCommandValidator : AbstractValidator<DeleteOtoriteKaydiCommand>
{
    public DeleteOtoriteKaydiCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
