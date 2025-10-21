using Application.Authorization;
using Application.Features.OtoriteKayitlari.Commands.Create;
using Application.Features.OtoriteKayitlari.Commands.Delete;
using Application.Features.OtoriteKayitlari.Commands.Update;
using Application.Features.OtoriteKayitlari.Queries.GetById;
using Application.Features.OtoriteKayitlari.Queries.GetList;
using Application.Features.OtoriteKayitlari.Queries.GetListByDynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireMinistry)]
public class OtoriteKayitlariController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedOtoriteKaydiResponse>> Add([FromBody] CreateOtoriteKaydiCommand command)
    {
        CreatedOtoriteKaydiResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedOtoriteKaydiResponse>> Update([FromBody] UpdateOtoriteKaydiCommand command)
    {
        UpdatedOtoriteKaydiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedOtoriteKaydiResponse>> Delete([FromRoute] Guid id)
    {
        DeleteOtoriteKaydiCommand command = new() { Id = id };

        DeletedOtoriteKaydiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdOtoriteKaydiResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdOtoriteKaydiQuery query = new() { Id = id };

        GetByIdOtoriteKaydiResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListOtoriteKaydiListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListOtoriteKaydiQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListOtoriteKaydiListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<ActionResult<GetListResponse<GetListOtoriteKaydiListItemDto>>> GetListByDynamic(
        [FromQuery] PageRequest pageRequest,
        [FromBody] DynamicQuery dynamicQuery
    )
    {
        GetListByDynamicOtoriteKaydiQuery query = new()
        {
            PageRequest = pageRequest,
            DynamicQuery = dynamicQuery
        };

        GetListResponse<GetListOtoriteKaydiListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
