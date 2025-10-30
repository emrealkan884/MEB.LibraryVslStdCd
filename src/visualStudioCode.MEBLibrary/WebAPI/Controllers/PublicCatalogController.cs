using System;
using Application.Features.PublicCatalog.Queries.GetDetail;
using Application.Features.PublicCatalog.Queries.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/public-catalog")]
[ApiController]
public class PublicCatalogController : BaseController
{
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<PublicCatalogSearchResponse>> Search([FromQuery] SearchPublicCatalogQuery query)
    {
        PublicCatalogSearchResponse response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<PublicCatalogDetailResponse>> GetDetail([FromRoute] Guid id)
    {
        PublicCatalogDetailResponse response = await Mediator.Send(new GetDetailPublicCatalogQuery { Id = id });
        return Ok(response);
    }
}
