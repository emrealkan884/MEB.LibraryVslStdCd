using Application.Features.Kutuphaneler.Commands.Create;
using Application.Features.Kutuphaneler.Commands.Delete;
using Application.Features.Kutuphaneler.Commands.Update;
using Application.Features.Kutuphaneler.Queries.GetById;
using Application.Features.Kutuphaneler.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KutuphanelerController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedKutuphaneResponse>> Add([FromBody] CreateKutuphaneCommand command)
    {
        CreatedKutuphaneResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedKutuphaneResponse>> Update([FromBody] UpdateKutuphaneCommand command)
    {
        UpdatedKutuphaneResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedKutuphaneResponse>> Delete([FromRoute] Guid id)
    {
        DeleteKutuphaneCommand command = new() { Id = id };

        DeletedKutuphaneResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdKutuphaneResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdKutuphaneQuery query = new() { Id = id };

        GetByIdKutuphaneResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListKutuphaneListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListKutuphaneQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListKutuphaneListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
