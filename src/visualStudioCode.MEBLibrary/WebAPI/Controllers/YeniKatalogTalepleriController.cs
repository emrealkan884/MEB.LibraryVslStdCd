using Application.Features.YeniKatalogTalepleri.Commands.Create;
using Application.Features.YeniKatalogTalepleri.Commands.Delete;
using Application.Features.YeniKatalogTalepleri.Commands.Update;
using Application.Features.YeniKatalogTalepleri.Queries.GetById;
using Application.Features.YeniKatalogTalepleri.Queries.GetList;
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
}
