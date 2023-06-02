using Ecommerce.Application.Features.Products.Queries.GetProductById;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.PaginationProducts;
using Ecommerce.Application.Features.Products.Queries.ViewModels;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("list", Name = "GetProductList")]
    [ProducesResponseType(typeof(IReadOnlyList<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProductList()
    {
        var query = new GetProductListQuery();
        var products = await _mediator.Send(query);

        return Ok(products);
    }

    [AllowAnonymous]
    [HttpGet("pagination", Name = "PaginationProduct")]
    [ProducesResponseType(typeof(PaginationViewModel<ProductViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationViewModel<ProductViewModel>>> PaginationProduct(
        [FromQuery] PaginationProductsQuery paginationProductsQuery
    )
    {
        paginationProductsQuery.Status = ProductStatus.Active;
        var paginationProduct = await _mediator.Send(paginationProductsQuery);

        return Ok(paginationProduct);
    }

    [AllowAnonymous]
    [HttpGet("{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductViewModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductViewModel>> GetProductById(int id)
    {
        var query = new GetProductByIdQuery(id);
        var productById = await _mediator.Send(query);

        return Ok(productById);
    }
}