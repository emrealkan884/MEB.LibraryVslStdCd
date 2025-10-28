using Application.Authorization;
using Application.Features.DeweySiniflamalari.Commands.Create;
using Application.Features.DeweySiniflamalari.Commands.Delete;
using Application.Features.DeweySiniflamalari.Commands.Update;
using Application.Features.DeweySiniflamalari.Queries.GetById;
using Application.Features.DeweySiniflamalari.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.BakanlikYetkisi)]
public class DeweySiniflamalarController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedDeweySiniflamaResponse>> Add([FromBody] CreateDeweySiniflamaCommand command)
    {
        CreatedDeweySiniflamaResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedDeweySiniflamaResponse>> Update([FromBody] UpdateDeweySiniflamaCommand command)
    {
        UpdatedDeweySiniflamaResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedDeweySiniflamaResponse>> Delete([FromRoute] Guid id)
    {
        DeleteDeweySiniflamaCommand command = new() { Id = id };

        DeletedDeweySiniflamaResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdDeweySiniflamaResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdDeweySiniflamaQuery query = new() { Id = id };

        GetByIdDeweySiniflamaResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListDeweySiniflamaListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListDeweySiniflamaQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListDeweySiniflamaListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
