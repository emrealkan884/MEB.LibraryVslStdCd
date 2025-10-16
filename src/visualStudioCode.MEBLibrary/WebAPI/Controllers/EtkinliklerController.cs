using Application.Features.Etkinlikler.Commands.Create;
using Application.Features.Etkinlikler.Commands.Delete;
using Application.Features.Etkinlikler.Commands.Update;
using Application.Features.Etkinlikler.Queries.GetById;
using Application.Features.Etkinlikler.Queries.GetList;
using Application.Features.Etkinlikler.Queries.GetListByDynamic;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EtkinliklerController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedEtkinlikResponse>> Add([FromBody] CreateEtkinlikCommand command)
    {
        CreatedEtkinlikResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedEtkinlikResponse>> Update([FromBody] UpdateEtkinlikCommand command)
    {
        UpdatedEtkinlikResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedEtkinlikResponse>> Delete([FromRoute] Guid id)
    {
        DeleteEtkinlikCommand command = new() { Id = id };

        DeletedEtkinlikResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdEtkinlikResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdEtkinlikQuery query = new() { Id = id };

        GetByIdEtkinlikResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListEtkinlikListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListEtkinlikQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListEtkinlikListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<ActionResult<GetListResponse<GetListEtkinlikListItemDto>>> GetListByDynamic(
        [FromQuery] PageRequest pageRequest,
        [FromBody] DynamicQuery dynamicQuery
    )
    {
        GetListByDynamicEtkinlikQuery query = new()
        {
            PageRequest = pageRequest,
            DynamicQuery = dynamicQuery
        };

        GetListResponse<GetListEtkinlikListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
