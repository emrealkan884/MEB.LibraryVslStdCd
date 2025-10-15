using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Etkinlikler.Queries.GetList;

public class GetListEtkinlikListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Baslik { get; set; }
    public string? Aciklama { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public string? Konum { get; set; }
    public string? AfisDosyasi { get; set; }
}
