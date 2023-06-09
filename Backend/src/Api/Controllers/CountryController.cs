using Ecommerce.Application.Features.Countries.Queries.GetCountryList;
using Ecommerce.Application.Features.Countries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CountryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CountryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet(Name = "GetCountries")]
    [ProducesResponseType(typeof(IReadOnlyList<CountryViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<CountryViewModel>>> GetCountries()
    {
        var query = new GetCountryListQuery();

        return Ok(await _mediator.Send(query));
    }
}