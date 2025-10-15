using Domain.Enums;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.MateryalFormatDetaylari.Commands.Update;

public class UpdatedMateryalFormatDetayResponse : IResponse
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
