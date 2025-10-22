using Application.Authorization;
using Application.Features.Raporlama.Queries.GetLoanUsageStatistics;
using Application.Features.Raporlama.Queries.GetOverdueLoans;
using Application.Features.Raporlama.Queries.GetLoanAggregates;
using Application.Services.Reporting;
using Application.Services.Reporting.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireProvinceOrAbove)]
public class RaporlamaController : BaseController
{
    private readonly IReportExportService _reportExportService;

    public RaporlamaController(IReportExportService reportExportService)
    {
        _reportExportService = reportExportService;
    }

    [HttpGet("odunc/gecikmis")]
    public async Task<ActionResult<List<OverdueLoanReportDto>>> GetOverdueLoans(
        [FromQuery] GetOverdueLoansReportQuery query
    )
    {
        List<OverdueLoanReportDto> response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("odunc/kullanim")]
    public async Task<ActionResult<List<LoanUsageStatisticsDto>>> GetUsageStatistics(
        [FromQuery] GetLoanUsageStatisticsQuery query
    )
    {
        List<LoanUsageStatisticsDto> response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpPost("odunc/gecikmis/export")]
    public async Task<ActionResult> ExportOverdueLoans(
        [FromBody] GetOverdueLoansReportQuery query,
        [FromQuery] ReportFormat format = ReportFormat.Csv
    )
    {
        List<OverdueLoanReportDto> data = await Mediator.Send(query);
        ReportExportResult exportResult = _reportExportService.Export(
            data,
            "overdue-loans",
            format,
            "Overdue Loans"
        );
        return File(exportResult.Content, exportResult.ContentType, exportResult.FileName);
    }

    [HttpPost("odunc/kullanim/export")]
    public async Task<ActionResult> ExportUsageStatistics(
        [FromBody] GetLoanUsageStatisticsQuery query,
        [FromQuery] ReportFormat format = ReportFormat.Csv
    )
    {
        List<LoanUsageStatisticsDto> data = await Mediator.Send(query);
        ReportExportResult exportResult = _reportExportService.Export(
            data,
            "loan-usage",
            format,
            "Loan Usage Statistics"
        );
        return File(exportResult.Content, exportResult.ContentType, exportResult.FileName);
    }

    [HttpGet("odunc/toplamlar")]
    public async Task<ActionResult<List<LoanAggregateDto>>> GetLoanAggregates([FromQuery] GetLoanAggregatesQuery query)
    {
        List<LoanAggregateDto> response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpPost("odunc/toplamlar/export")]
    public async Task<ActionResult> ExportLoanAggregates(
        [FromBody] GetLoanAggregatesQuery query,
        [FromQuery] ReportFormat format = ReportFormat.Csv
    )
    {
        List<LoanAggregateDto> data = await Mediator.Send(query);
        string title = $"Loan Aggregates - {query.Dimension}";
        ReportExportResult exportResult = _reportExportService.Export(
            data,
            "loan-aggregates",
            format,
            title
        );
        return File(exportResult.Content, exportResult.ContentType, exportResult.FileName);
    }
}


