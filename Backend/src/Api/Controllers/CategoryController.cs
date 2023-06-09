using Ecommerce.Application.Features.Categories;
using Ecommerce.Application.Features.Categories.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("list", Name = "GetCategoryList")]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<CategoryViewModel>>> GetCategoryList()
    {
        var query = new GetCategoryListQuery();

        return Ok(await _mediator.Send(query));
    }
}