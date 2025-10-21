using Application.Authorization;
using Application.Features.MateryalEtiketleri.Commands.Create;
using Application.Features.MateryalEtiketleri.Commands.Delete;
using Application.Features.MateryalEtiketleri.Commands.Update;
using Application.Features.MateryalEtiketleri.Queries.GetById;
using Application.Features.MateryalEtiketleri.Queries.GetList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireSchoolOrAbove)]
public class MateryalEtiketlerController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedMateryalEtiketResponse>> Add([FromBody] CreateMateryalEtiketCommand command)
    {
        CreatedMateryalEtiketResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedMateryalEtiketResponse>> Update([FromBody] UpdateMateryalEtiketCommand command)
    {
        UpdatedMateryalEtiketResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedMateryalEtiketResponse>> Delete([FromRoute] Guid id)
    {
        DeleteMateryalEtiketCommand command = new() { Id = id };

        DeletedMateryalEtiketResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdMateryalEtiketResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdMateryalEtiketQuery query = new() { Id = id };

        GetByIdMateryalEtiketResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListMateryalEtiketListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListMateryalEtiketQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListMateryalEtiketListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
