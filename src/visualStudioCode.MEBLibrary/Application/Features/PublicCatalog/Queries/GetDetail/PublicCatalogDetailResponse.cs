namespace Application.Features.PublicCatalog.Queries.GetDetail;

public class PublicCatalogDetailResponse
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Subtitle { get; set; }
    public IReadOnlyCollection<string> Authors { get; set; } = Array.Empty<string>();
    public string MaterialType { get; set; } = string.Empty;
    public string MaterialCategory { get; set; } = string.Empty;
    public string? Language { get; set; }
    public int? PublicationYear { get; set; }
    public string? Publisher { get; set; }
    public string? PublicationPlace { get; set; }
    public string? Isbn { get; set; }
    public string? Summary { get; set; }
    public string? Notes { get; set; }
    public string? DeweyCode { get; set; }
    public string? DeweyTitle { get; set; }
    public string? Marc21 { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IReadOnlyCollection<PublicCatalogDigitalResourceDto> DigitalResources { get; set; } = Array.Empty<PublicCatalogDigitalResourceDto>();
    public IReadOnlyCollection<PublicCatalogAvailabilityDto> Availability { get; set; } = Array.Empty<PublicCatalogAvailabilityDto>();
}