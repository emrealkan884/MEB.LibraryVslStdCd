using Application.Authorization;
using Application.Features.Materyaller.Commands.Create;
using Application.Features.Materyaller.Commands.Delete;
using Application.Features.Materyaller.Commands.Update;
using Application.Features.Materyaller.Queries.GetById;
using Application.Features.Materyaller.Queries.GetList;
using Application.Features.Materyaller.Queries.GetListByDynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireSchoolOrAbove)]
public class MateryallerController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedMateryalResponse>> Add([FromBody] CreateMateryalCommand command)
    {
        CreatedMateryalResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedMateryalResponse>> Update([FromBody] UpdateMateryalCommand command)
    {
        UpdatedMateryalResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedMateryalResponse>> Delete([FromRoute] Guid id)
    {
        DeleteMateryalCommand command = new() { Id = id };

        DeletedMateryalResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdMateryalResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdMateryalQuery query = new() { Id = id };

        GetByIdMateryalResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListMateryalListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListMateryalQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListMateryalListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<ActionResult<GetListResponse<GetListMateryalListItemDto>>> GetListByDynamic(
        [FromQuery] PageRequest pageRequest,
        [FromBody] DynamicQuery dynamicQuery
    )
    {
        GetListByDynamicMateryalQuery query = new()
        {
            PageRequest = pageRequest,
            DynamicQuery = dynamicQuery
        };

        GetListResponse<GetListMateryalListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}

