using NArchitecture.Core.Application.Responses;

namespace Application.Features.KatalogKonulari.Queries.GetById;

public class GetByIdKatalogKonuResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public required string KonuBasligi { get; set; }
    public Guid? OtoriteKaydiId { get; set; }
}
