using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.IO;
using Application.Services.Reporting;
using Application.Services.Reporting.Models;

namespace Infrastructure.Adapters.Reporting;

public class ReportExportService : IReportExportService
{
    public ReportExportResult Export<T>(IEnumerable<T> data, string fileNamePrefix, ReportFormat format, string? title = null)
    {
        List<T> records = data.ToList();
        PropertyInfo[] properties = typeof(T).GetProperties();
        string reportTitle = string.IsNullOrWhiteSpace(title) ? fileNamePrefix : title;

        return format switch
        {
            ReportFormat.Excel => ExportToExcel(records, properties, fileNamePrefix, reportTitle),
            ReportFormat.Pdf => ExportToPdf(records, properties, fileNamePrefix, reportTitle),
            _ => ExportToCsv(records, properties, fileNamePrefix)
        };
    }

    private static ReportExportResult ExportToCsv<T>(IReadOnlyCollection<T> records, PropertyInfo[] properties, string fileNamePrefix)
    {
        StringBuilder builder = new();

        if (records.Count == 0)
        {
            builder.AppendLine("NoData");
        }
        else
        {
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
            Content = content,
            Format = ReportFormat.Csv
        };
    }

    private static ReportExportResult ExportToExcel<T>(IReadOnlyCollection<T> records, PropertyInfo[] properties, string fileNamePrefix, string title)
    {
        StringBuilder builder = new();
        builder.AppendLine("<html><head><meta charset=\"UTF-8\"></head><body>");
        builder.AppendLine($"<h3>{WebUtility.HtmlEncode(title)}</h3>");
        builder.AppendLine("<table border=\"1\" cellspacing=\"0\" cellpadding=\"3\">");
        builder.AppendLine("<tr>");
        foreach (PropertyInfo property in properties)
            builder.AppendLine($"<th>{WebUtility.HtmlEncode(property.Name)}</th>");
        builder.AppendLine("</tr>");

        foreach (T record in records)
        {
            builder.AppendLine("<tr>");
            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(record);
                string cellValue = value switch
                {
                    null => string.Empty,
                    DateTime dateTime => dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    DateTimeOffset offset => offset.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    _ => value.ToString() ?? string.Empty
                };
                builder.AppendLine($"<td>{WebUtility.HtmlEncode(cellValue)}</td>");
            }

            builder.AppendLine("</tr>");
        }

        if (records.Count == 0)
        {
            builder.AppendLine($"<tr><td colspan=\"{properties.Length}\">No data</td></tr>");
        }

        builder.AppendLine("</table>");
        builder.AppendLine("</body></html>");

        byte[] content = Encoding.UTF8.GetBytes(builder.ToString());
        string fileName = $"{fileNamePrefix}_{DateTime.UtcNow:yyyyMMddHHmmss}.xls";

        return new ReportExportResult
        {
            FileName = fileName,
            ContentType = "application/vnd.ms-excel",
            Content = content,
            Format = ReportFormat.Excel
        };
    }

    private static ReportExportResult ExportToPdf<T>(IReadOnlyCollection<T> records, PropertyInfo[] properties, string fileNamePrefix, string title)
    {
        string fileName = $"{fileNamePrefix}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
        byte[] content = BuildSimplePdfDocument(records, properties, title);

        return new ReportExportResult
        {
            FileName = fileName,
            ContentType = "application/pdf",
            Content = content,
            Format = ReportFormat.Pdf
        };
    }

    private static string EscapeCsv(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";

        return value;
    }

    private static byte[] BuildSimplePdfDocument<T>(IReadOnlyCollection<T> records, PropertyInfo[] properties, string title)
    {
        List<string> lines = new();
        lines.Add(title);
        lines.Add(string.Empty);

        if (records.Count == 0)
        {
            lines.Add("No data");
        }
        else
        {
            string header = string.Join(" | ", properties.Select(p => p.Name));
            lines.Add(header);
            lines.Add(new string('-', header.Length));

            foreach (T record in records)
            {
                string row = string.Join(
                    " | ",
                    properties.Select(prop =>
                    {
                        object? value = prop.GetValue(record);
                        return value switch
                        {
                            null => string.Empty,
                            DateTime dateTime => dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            DateTimeOffset offset => offset.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            _ => value.ToString() ?? string.Empty
                        };
                    })
                );
                lines.Add(row);
            }
        }

        string contentStream = BuildPdfContentStream(lines);
        return AssemblePdf(contentStream);
    }

    private static string BuildPdfContentStream(IEnumerable<string> lines)
    {
        StringBuilder builder = new();
        builder.AppendLine("BT");
        builder.AppendLine("/F1 12 Tf");

        int lineIndex = 0;
        foreach (string line in lines)
        {
            int yPosition = 800 - (lineIndex * 16);
            builder.AppendLine($"1 0 0 1 50 {yPosition} Tm");
            builder.AppendLine($"({EscapePdfText(line)}) Tj");
            lineIndex++;
        }

        builder.AppendLine("ET");
        return builder.ToString();
    }

    private static string EscapePdfText(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("(", "\\(")
            .Replace(")", "\\)");
    }

    private static byte[] AssemblePdf(string contentStream)
    {
        Encoding encoding = Encoding.ASCII;

        string catalog = "1 0 obj << /Type /Catalog /Pages 2 0 R >> endobj\n";
        string pages = "2 0 obj << /Type /Pages /Kids [3 0 R] /Count 1 >> endobj\n";
        string page =
            "3 0 obj << /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Contents 4 0 R /Resources << /Font << /F1 5 0 R >> >> >> endobj\n";
        string content =
            $"4 0 obj << /Length {contentStream.Length} >> stream\n{contentStream}endstream\nendobj\n";
        string font = "5 0 obj << /Type /Font /Subtype /Type1 /BaseFont /Helvetica >> endobj\n";

        List<string> objects = [catalog, pages, page, content, font];

        using MemoryStream ms = new();
        void Write(string text) => ms.Write(encoding.GetBytes(text));

        Write("%PDF-1.4\n");

        List<long> offsets = [0];
        foreach (string obj in objects)
        {
            offsets.Add(ms.Position);
            Write(obj);
        }

        long xrefPosition = ms.Position;

        Write($"xref\n0 {offsets.Count}\n");
        Write("0000000000 65535 f \n");
        for (int i = 1; i < offsets.Count; i++)
            Write($"{offsets[i]:D10} 00000 n \n");

        Write($"trailer\n<< /Size {offsets.Count} /Root 1 0 R >>\n");
        Write("startxref\n");
        Write($"{xrefPosition}\n");
        Write("%%EOF");

        return ms.ToArray();
    }
}
