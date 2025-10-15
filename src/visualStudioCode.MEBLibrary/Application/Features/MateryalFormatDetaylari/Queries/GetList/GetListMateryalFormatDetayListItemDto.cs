using Domain.Enums;
using NArchitecture.Core.Application.Dtos;

namespace Application.Features.MateryalFormatDetaylari.Queries.GetList;

public class GetListMateryalFormatDetayListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public MateryalTuru MateryalTuru { get; set; }
    public string? FizikselTanimi { get; set; }
    public string? SureBilgisi { get; set; }
    public string? FormatBilgisi { get; set; }
    public string? Dil { get; set; }
    public string? ErişimBilgisi { get; set; }
}
