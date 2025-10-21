using Application.Authorization;
using Application.Features.MateryalFormatDetaylari.Commands.Create;
using Application.Features.MateryalFormatDetaylari.Commands.Delete;
using Application.Features.MateryalFormatDetaylari.Commands.Update;
using Application.Features.MateryalFormatDetaylari.Queries.GetById;
using Application.Features.MateryalFormatDetaylari.Queries.GetList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireSchoolOrAbove)]
public class MateryalFormatDetaylarController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedMateryalFormatDetayResponse>> Add([FromBody] CreateMateryalFormatDetayCommand command)
    {
        CreatedMateryalFormatDetayResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedMateryalFormatDetayResponse>> Update([FromBody] UpdateMateryalFormatDetayCommand command)
    {
        UpdatedMateryalFormatDetayResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedMateryalFormatDetayResponse>> Delete([FromRoute] Guid id)
    {
        DeleteMateryalFormatDetayCommand command = new() { Id = id };

        DeletedMateryalFormatDetayResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdMateryalFormatDetayResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdMateryalFormatDetayQuery query = new() { Id = id };

        GetByIdMateryalFormatDetayResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListMateryalFormatDetayListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListMateryalFormatDetayQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListMateryalFormatDetayListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }
}
