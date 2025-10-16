using System.Globalization;
using System.Text;
using Application.Services.Reporting;
using Application.Services.Reporting.Models;

namespace Infrastructure.Adapters.Reporting;

public class CsvReportExportService : IReportExportService
{
    public ReportExportResult ExportToCsv<T>(IEnumerable<T> data, string fileNamePrefix)
    {
        StringBuilder builder = new();
        var records = data.ToList();

        if (records.Count == 0)
        {
            builder.AppendLine("NoData");
        }
        else
        {
            var properties = typeof(T).GetProperties();
            builder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            foreach (T record in records)
            {
                string line = string.Join(
                    ",",
                    properties.Select(prop =>
                    {
                        object? value = prop.GetValue(record);
                        return value switch
                        {
                            null => string.Empty,
                            DateTime dateTime => dateTime.ToString("o", CultureInfo.InvariantCulture),
                            DateTimeOffset offset => offset.ToString("o", CultureInfo.InvariantCulture),
                            _ => EscapeCsv(value.ToString() ?? string.Empty)
                        };
                    })
                );
                builder.AppendLine(line);
            }
        }

        byte[] content = Encoding.UTF8.GetBytes(builder.ToString());
        string fileName = $"{fileNamePrefix}_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

        return new ReportExportResult
        {
            FileName = fileName,
            ContentType = "text/csv",
            Content = content
        };
    }

    private static string EscapeCsv(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }
}
