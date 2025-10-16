namespace Application.Services.Reporting.Models;

public class ReportExportResult
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Content { get; init; }
}
