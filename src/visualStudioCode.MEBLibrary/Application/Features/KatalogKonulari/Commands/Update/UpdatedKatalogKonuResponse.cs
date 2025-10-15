using NArchitecture.Core.Application.Responses;

namespace Application.Features.KatalogKonulari.Commands.Update;

public class UpdatedKatalogKonuResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public required string KonuBasligi { get; set; }
    public Guid? OtoriteKaydiId { get; set; }
}
