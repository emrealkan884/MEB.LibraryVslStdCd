using Application.Features.KutuphaneBolumleri.Commands.Create;
using Application.Features.KutuphaneBolumleri.Commands.Delete;
using Application.Features.KutuphaneBolumleri.Commands.Update;
using Application.Features.KutuphaneBolumleri.Queries.GetById;
using Application.Features.KutuphaneBolumleri.Queries.GetList;
using Application.Features.KutuphaneBolumleri.Queries.GetListByDynamic;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KutuphaneBolumleriController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedKutuphaneBolumuResponse>> Add([FromBody] CreateKutuphaneBolumuCommand command)
    {
        CreatedKutuphaneBolumuResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedKutuphaneBolumuResponse>> Update([FromBody] UpdateKutuphaneBolumuCommand command)
    {
        UpdatedKutuphaneBolumuResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedKutuphaneBolumuResponse>> Delete([FromRoute] Guid id)
    {
        DeleteKutuphaneBolumuCommand command = new() { Id = id };

        DeletedKutuphaneBolumuResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdKutuphaneBolumuResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdKutuphaneBolumuQuery query = new() { Id = id };

        GetByIdKutuphaneBolumuResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListKutuphaneBolumuListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListKutuphaneBolumuQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListKutuphaneBolumuListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<ActionResult<GetListResponse<GetListKutuphaneBolumuListItemDto>>> GetListByDynamic(
        [FromQuery] PageRequest pageRequest,
        [FromBody] DynamicQuery dynamicQuery
    )
    {
        GetListByDynamicKutuphaneBolumuQuery query = new()
        {
            PageRequest = pageRequest,
            DynamicQuery = dynamicQuery
        };

        GetListResponse<GetListKutuphaneBolumuListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
