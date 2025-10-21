using Application.Authorization;
using Application.Features.Raflar.Commands.Create;
using Application.Features.Raflar.Commands.Delete;
using Application.Features.Raflar.Commands.Update;
using Application.Features.Raflar.Queries.GetById;
using Application.Features.Raflar.Queries.GetList;
using Application.Features.Raflar.Queries.GetListByDynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireSchoolOrAbove)]
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

    [HttpPost("GetListByDynamic")]
    public async Task<ActionResult<GetListResponse<GetListRafListItemDto>>> GetListByDynamic(
        [FromQuery] PageRequest pageRequest,
        [FromBody] DynamicQuery dynamicQuery
    )
    {
        GetListByDynamicRafQuery query = new()
        {
            PageRequest = pageRequest,
            DynamicQuery = dynamicQuery
        };

        GetListResponse<GetListRafListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
