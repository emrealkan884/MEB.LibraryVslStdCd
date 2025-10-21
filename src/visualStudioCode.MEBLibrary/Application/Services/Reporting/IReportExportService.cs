using Application.Services.Reporting.Models;

namespace Application.Services.Reporting;

public interface IReportExportService
{
    ReportExportResult Export<T>(IEnumerable<T> data, string fileNamePrefix, ReportFormat format, string? title = null);
}
