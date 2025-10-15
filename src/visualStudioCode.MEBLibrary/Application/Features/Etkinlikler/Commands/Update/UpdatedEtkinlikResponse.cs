using NArchitecture.Core.Application.Responses;

namespace Application.Features.Etkinlikler.Commands.Update;

public class UpdatedEtkinlikResponse : IResponse
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
