using Application.Features.Nushalar.Commands.Create;
using Application.Features.Nushalar.Commands.Delete;
using Application.Features.Nushalar.Commands.Update;
using Application.Features.Nushalar.Queries.GetById;
using Application.Features.Nushalar.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NushalarController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedNushaResponse>> Add([FromBody] CreateNushaCommand command)
    {
        CreatedNushaResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedNushaResponse>> Update([FromBody] UpdateNushaCommand command)
    {
        UpdatedNushaResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedNushaResponse>> Delete([FromRoute] Guid id)
    {
        DeleteNushaCommand command = new() { Id = id };

        DeletedNushaResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdNushaResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdNushaQuery query = new() { Id = id };

        GetByIdNushaResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListNushaListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListNushaQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListNushaListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
