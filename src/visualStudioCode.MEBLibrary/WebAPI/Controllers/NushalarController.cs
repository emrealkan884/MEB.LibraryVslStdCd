using Application.Authorization;
using Application.Features.Nushalar.Commands.Create;
using Application.Features.Nushalar.Commands.Delete;
using Application.Features.Nushalar.Commands.Update;
using Application.Features.Nushalar.Queries.GetById;
using Application.Features.Nushalar.Queries.GetList;
using Application.Features.Nushalar.Queries.GetListByDynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.OkulYetkisiVeyaUstu)]
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

    [HttpPost("GetListByDynamic")]
    public async Task<ActionResult<GetListResponse<GetListNushaListItemDto>>> GetListByDynamic(
        [FromQuery] PageRequest pageRequest,
        [FromBody] DynamicQuery dynamicQuery
    )
    {
        GetListByDynamicNushaQuery query = new()
        {
            PageRequest = pageRequest,
            DynamicQuery = dynamicQuery
        };

        GetListResponse<GetListNushaListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
