namespace Application.Features.PublicCatalog.Queries.GetDetail;

public class PublicCatalogAvailabilityDto
{
    public Guid LibraryId { get; set; }
    public string LibraryName { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }
    public IReadOnlyCollection<string> Sections { get; set; } = Array.Empty<string>();
    public bool IsReservable { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public string? Notes { get; set; }
}