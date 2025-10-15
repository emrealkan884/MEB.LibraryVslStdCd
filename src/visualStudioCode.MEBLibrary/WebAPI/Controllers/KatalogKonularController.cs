using Application.Features.KatalogKonulari.Commands.Create;
using Application.Features.KatalogKonulari.Commands.Delete;
using Application.Features.KatalogKonulari.Commands.Update;
using Application.Features.KatalogKonulari.Queries.GetById;
using Application.Features.KatalogKonulari.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KatalogKonularController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedKatalogKonuResponse>> Add([FromBody] CreateKatalogKonuCommand command)
    {
        CreatedKatalogKonuResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedKatalogKonuResponse>> Update([FromBody] UpdateKatalogKonuCommand command)
    {
        UpdatedKatalogKonuResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedKatalogKonuResponse>> Delete([FromRoute] Guid id)
    {
        DeleteKatalogKonuCommand command = new() { Id = id };

        DeletedKatalogKonuResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdKatalogKonuResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdKatalogKonuQuery query = new() { Id = id };

        GetByIdKatalogKonuResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListKatalogKonuListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListKatalogKonuQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListKatalogKonuListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
