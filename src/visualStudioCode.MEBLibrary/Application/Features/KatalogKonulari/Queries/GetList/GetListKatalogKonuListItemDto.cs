using NArchitecture.Core.Application.Dtos;

namespace Application.Features.KatalogKonulari.Queries.GetList;

public class GetListKatalogKonuListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public required string KonuBasligi { get; set; }
    public Guid? OtoriteKaydiId { get; set; }
}
