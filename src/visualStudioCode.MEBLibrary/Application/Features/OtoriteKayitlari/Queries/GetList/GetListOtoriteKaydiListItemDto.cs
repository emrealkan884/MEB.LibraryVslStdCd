using Domain.Enums;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.OtoriteKayitlari.Queries.GetList;

public class GetListOtoriteKaydiListItemDto : IDto
{
    public Guid Id { get; set; }
    public required string YetkiliBaslik { get; set; }
    public OtoriteTuru OtoriteTuru { get; set; }
    public string? AlternatifBasliklar { get; set; }
    public string? BagliTerimler { get; set; }
    public string? Aciklama { get; set; }
    public string? HariciKayitNo { get; set; }
}
