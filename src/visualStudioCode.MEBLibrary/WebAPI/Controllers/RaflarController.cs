using Application.Features.Raflar.Commands.Create;
using Application.Features.Raflar.Commands.Delete;
using Application.Features.Raflar.Commands.Update;
using Application.Features.Raflar.Queries.GetById;
using Application.Features.Raflar.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RaflarController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedRafResponse>> Add([FromBody] CreateRafCommand command)
    {
        CreatedRafResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedRafResponse>> Update([FromBody] UpdateRafCommand command)
    {
        UpdatedRafResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedRafResponse>> Delete([FromRoute] Guid id)
    {
        DeleteRafCommand command = new() { Id = id };

        DeletedRafResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdRafResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdRafQuery query = new() { Id = id };

        GetByIdRafResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListRafListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListRafQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListRafListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
