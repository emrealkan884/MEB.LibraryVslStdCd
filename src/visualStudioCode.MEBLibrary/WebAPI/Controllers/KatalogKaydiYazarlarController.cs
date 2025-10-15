using Application.Features.KatalogKaydiYazarlar.Commands.Create;
using Application.Features.KatalogKaydiYazarlar.Commands.Delete;
using Application.Features.KatalogKaydiYazarlar.Commands.Update;
using Application.Features.KatalogKaydiYazarlar.Queries.GetById;
using Application.Features.KatalogKaydiYazarlar.Queries.GetList;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KatalogKaydiYazarlarController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedKatalogKaydiYazarResponse>> Add([FromBody] CreateKatalogKaydiYazarCommand command)
    {
        CreatedKatalogKaydiYazarResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedKatalogKaydiYazarResponse>> Update([FromBody] UpdateKatalogKaydiYazarCommand command)
    {
        UpdatedKatalogKaydiYazarResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedKatalogKaydiYazarResponse>> Delete([FromRoute] Guid id)
    {
        DeleteKatalogKaydiYazarCommand command = new() { Id = id };

        DeletedKatalogKaydiYazarResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdKatalogKaydiYazarResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdKatalogKaydiYazarQuery query = new() { Id = id };

        GetByIdKatalogKaydiYazarResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListKatalogKaydiYazarListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListKatalogKaydiYazarQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListKatalogKaydiYazarListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
