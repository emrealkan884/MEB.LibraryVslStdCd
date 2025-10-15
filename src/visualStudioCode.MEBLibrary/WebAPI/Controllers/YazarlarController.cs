using Application.Features.Yazarlar.Commands.Create;
using Application.Features.Yazarlar.Commands.Delete;
using Application.Features.Yazarlar.Commands.Update;
using Application.Features.Yazarlar.Queries.GetById;
using Application.Features.Yazarlar.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class YazarlarController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedYazarResponse>> Add([FromBody] CreateYazarCommand command)
    {
        CreatedYazarResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedYazarResponse>> Update([FromBody] UpdateYazarCommand command)
    {
        UpdatedYazarResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedYazarResponse>> Delete([FromRoute] Guid id)
    {
        DeleteYazarCommand command = new() { Id = id };

        DeletedYazarResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdYazarResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdYazarQuery query = new() { Id = id };

        GetByIdYazarResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListYazarListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListYazarQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListYazarListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}