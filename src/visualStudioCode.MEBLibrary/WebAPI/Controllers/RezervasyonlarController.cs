using Application.Authorization;
using Application.Features.Rezervasyonlar.Commands.Create;
using Application.Features.Rezervasyonlar.Commands.Delete;
using Application.Features.Rezervasyonlar.Commands.Update;
using Application.Features.Rezervasyonlar.Queries.GetById;
using Application.Features.Rezervasyonlar.Queries.GetList;
using Application.Features.Rezervasyonlar.Queries.GetListByDynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.OkulYetkisiVeyaUstu)]
public class RezervasyonlarController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedRezervasyonResponse>> Add([FromBody] CreateRezervasyonCommand command)
    {
        CreatedRezervasyonResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedRezervasyonResponse>> Update([FromBody] UpdateRezervasyonCommand command)
    {
        UpdatedRezervasyonResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedRezervasyonResponse>> Delete([FromRoute] Guid id)
    {
        DeleteRezervasyonCommand command = new() { Id = id };

        DeletedRezervasyonResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdRezervasyonResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdRezervasyonQuery query = new() { Id = id };

        GetByIdRezervasyonResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListRezervasyonListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListRezervasyonQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListRezervasyonListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<ActionResult<GetListResponse<GetListRezervasyonListItemDto>>> GetListByDynamic(
        [FromQuery] PageRequest pageRequest,
        [FromBody] DynamicQuery dynamicQuery
    )
    {
        GetListByDynamicRezervasyonQuery query = new()
        {
            PageRequest = pageRequest,
            DynamicQuery = dynamicQuery
        };

        GetListResponse<GetListRezervasyonListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
