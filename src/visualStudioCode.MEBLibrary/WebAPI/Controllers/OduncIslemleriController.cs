using Application.Features.OduncIslemler.Commands.Create;
using Application.Features.OduncIslemler.Commands.Delete;
using Application.Features.OduncIslemler.Commands.Return;
using Application.Features.OduncIslemler.Commands.Update;
using Application.Features.OduncIslemler.Queries.GetById;
using Application.Features.OduncIslemler.Queries.GetList;
using Application.Features.OduncIslemler.Queries.GetListByDynamic;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OduncIslemleriController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedOduncIslemiResponse>> Add([FromBody] CreateOduncIslemiCommand command)
    {
        CreatedOduncIslemiResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedOduncIslemiResponse>> Update([FromBody] UpdateOduncIslemiCommand command)
    {
        UpdatedOduncIslemiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedOduncIslemiResponse>> Delete([FromRoute] Guid id)
    {
        DeleteOduncIslemiCommand command = new() { Id = id };

        DeletedOduncIslemiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdOduncIslemiResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdOduncIslemiQuery query = new() { Id = id };

        GetByIdOduncIslemiResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListOduncIslemiListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListOduncIslemiQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListOduncIslemiListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<ActionResult<GetListResponse<GetListOduncIslemiListItemDto>>> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] dynamic dynamicQuery)
    {
        GetListByDynamicOduncIslemiQuery query = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };

        GetListResponse<GetListOduncIslemiListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPut("{id}/iade")]
    public async Task<IActionResult> Return([FromRoute] Guid id, [FromBody] ReturnOduncIslemiCommand command)
    {
        if (command == null)
        {
            command = new ReturnOduncIslemiCommand();
        }
        command.Id = id;

        await Mediator.Send(command);

        return NoContent(); // Başarılı iade için 204 No Content
    }
}
