using NArchitecture.Core.Application.Responses;

namespace Application.Features.OtoriteKayitlari.Commands.Delete;

public class DeletedOtoriteKaydiResponse : IResponse
{
    public Guid Id { get; set; }
}
