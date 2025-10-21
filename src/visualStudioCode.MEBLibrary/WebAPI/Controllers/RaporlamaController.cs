using Application.Authorization;
using Application.Features.Raporlama.Queries.GetLoanUsageStatistics;
using Application.Features.Raporlama.Queries.GetOverdueLoans;
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

    [HttpGet("odunc/overdue")]
    public async Task<ActionResult<List<OverdueLoanReportDto>>> GetOverdueLoans(
        [FromQuery] GetOverdueLoansReportQuery query
    )
    {
        List<OverdueLoanReportDto> response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("odunc/usage")]
    public async Task<ActionResult<List<LoanUsageStatisticsDto>>> GetUsageStatistics(
        [FromQuery] GetLoanUsageStatisticsQuery query
    )
    {
        List<LoanUsageStatisticsDto> response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpPost("odunc/overdue/export")]
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

    [HttpPost("odunc/usage/export")]
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
}
