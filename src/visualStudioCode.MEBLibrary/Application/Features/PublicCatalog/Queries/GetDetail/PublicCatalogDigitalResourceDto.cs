namespace Application.Features.PublicCatalog.Queries.GetDetail;

public class PublicCatalogDigitalResourceDto
{
    public string Format { get; set; } = string.Empty;
    public string AccessInformation { get; set; } = string.Empty;
    public string? Language { get; set; }
    public string? Description { get; set; }
}