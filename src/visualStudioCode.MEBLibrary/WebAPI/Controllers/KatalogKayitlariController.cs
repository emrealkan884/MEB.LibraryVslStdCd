using Application.Features.KatalogKayitlari.Commands.Create;
using Application.Features.KatalogKayitlari.Commands.Delete;
using Application.Features.KatalogKayitlari.Commands.Update;
using Application.Features.KatalogKayitlari.Queries.GetById;
using Application.Features.KatalogKayitlari.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KatalogKayitlariController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedKatalogKaydiResponse>> Add([FromBody] CreateKatalogKaydiCommand command)
    {
        CreatedKatalogKaydiResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedKatalogKaydiResponse>> Update([FromBody] UpdateKatalogKaydiCommand command)
    {
        UpdatedKatalogKaydiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedKatalogKaydiResponse>> Delete([FromRoute] Guid id)
    {
        DeleteKatalogKaydiCommand command = new() { Id = id };

        DeletedKatalogKaydiResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdKatalogKaydiResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdKatalogKaydiQuery query = new() { Id = id };

        GetByIdKatalogKaydiResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListKatalogKaydiListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListKatalogKaydiQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListKatalogKaydiListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
