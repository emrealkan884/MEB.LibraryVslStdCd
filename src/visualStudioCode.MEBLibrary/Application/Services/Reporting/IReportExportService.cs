using Application.Services.Reporting.Models;

namespace Application.Services.Reporting;

public interface IReportExportService
{
    ReportExportResult ExportToCsv<T>(IEnumerable<T> data, string fileNamePrefix);
}
