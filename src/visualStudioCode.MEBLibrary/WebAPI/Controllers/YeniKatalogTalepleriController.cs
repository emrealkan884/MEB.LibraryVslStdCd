using Application.Features.YeniKatalogTalepleri.Commands.Approve;
using Application.Features.YeniKatalogTalepleri.Commands.Create;
using Application.Features.YeniKatalogTalepleri.Commands.Delete;
using Application.Features.YeniKatalogTalepleri.Commands.Reject;
using Application.Features.YeniKatalogTalepleri.Commands.Update;
using Application.Features.YeniKatalogTalepleri.Queries.GetById;
using Application.Features.YeniKatalogTalepleri.Queries.GetList;
using Domain.Enums;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class YeniKatalogTalepleriController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedYeniKatalogTalebiResponse>> Add([FromBody] CreateYeniKatalogTalebiCommand command)
    {
        CreatedYeniKatalogTalebiResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedYeniKatalogTalebiResponse>> Update([FromBody] UpdateYeniKatalogTalebiCommand command)
    {
        UpdatedYeniKatalogTalebiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedYeniKatalogTalebiResponse>> Delete([FromRoute] Guid id)
    {
        DeleteYeniKatalogTalebiCommand command = new() { Id = id };

        DeletedYeniKatalogTalebiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdYeniKatalogTalebiResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdYeniKatalogTalebiQuery query = new() { Id = id };

        GetByIdYeniKatalogTalebiResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListYeniKatalogTalebiListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListYeniKatalogTalebiQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListYeniKatalogTalebiListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<ApprovedYeniKatalogTalebiResponse>> Approve(
        [FromRoute] Guid id,
        [FromBody] ApproveYeniKatalogTalebiRequest request
    )
    {
        ApproveYeniKatalogTalebiCommand command = new()
        {
            Id = id,
            OnaylayanKutuphaneId = request.OnaylayanKutuphaneId,
            MateryalTuru = request.MateryalTuru,
            MateryalAltTuru = request.MateryalAltTuru,
            DeweySiniflamaId = request.DeweySiniflamaId,
            Marc21Verisi = request.Marc21Verisi,
            RdaUyumlu = request.RdaUyumlu,
            SayfaSayisi = request.SayfaSayisi,
            Notlar = request.Notlar
        };

        ApprovedYeniKatalogTalebiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<RejectedYeniKatalogTalebiResponse>> Reject(
        [FromRoute] Guid id,
        [FromBody] RejectYeniKatalogTalebiRequest request
    )
    {
        RejectYeniKatalogTalebiCommand command = new() { Id = id, Gerekce = request.Gerekce };

        RejectedYeniKatalogTalebiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    public sealed record ApproveYeniKatalogTalebiRequest(
        Guid OnaylayanKutuphaneId,
        MateryalTuru MateryalTuru,
        string? MateryalAltTuru,
        Guid? DeweySiniflamaId,
        string? Marc21Verisi,
        bool RdaUyumlu,
        int? SayfaSayisi,
        string? Notlar
    );

    public sealed record RejectYeniKatalogTalebiRequest(string Gerekce);
}
