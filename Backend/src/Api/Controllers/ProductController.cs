using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.DeleteProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Queries.GetProductById;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.PaginationProducts;
using Ecommerce.Application.Features.Products.Queries.ViewModels;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using Ecommerce.Application.Models.ImageManagement;
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
    private readonly IManageImageService _manageImageService;

    public ProductController(IMediator mediator, IManageImageService manageImageService)
    {
        _mediator = mediator;
        _manageImageService = manageImageService;
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

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("paginationAdmin", Name = "PaginationProductAdmin")]
    [ProducesResponseType(typeof(PaginationViewModel<ProductViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationViewModel<ProductViewModel>>> PaginationAdmin(
        [FromQuery] PaginationProductsQuery paginationProductsQuery
    )
    {
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

    [Authorize(Roles = Role.ADMIN)]
    [HttpPost("create", Name = "CreateProduct")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductViewModel>> CreateProduct([FromForm] CreateProductCommand request)
    {
        var listPhotoUrls = new List<CreateProductImageCommand>();

        if (request.Photos is not null)
        {
            foreach (var photo in request.Photos)
            {
                var resultImage = await _manageImageService.UploadImage(new ImageData
                {
                    ImageStream = photo.OpenReadStream(),
                    Name = photo.Name
                });

                var photoCommand = new CreateProductImageCommand
                {
                    PublicCode = resultImage.PublicId,
                    Url = resultImage.Url
                };

                listPhotoUrls.Add(photoCommand);
            }

            request.ImageUrls = listPhotoUrls;
        }

        return await _mediator.Send(request);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpPut("update", Name = "UpdateProduct")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductViewModel>> UpdateProduct([FromForm] UpdateProductCommand request)
    {
        var listPhotoUrls = new List<CreateProductImageCommand>();

        if (request.Photos is not null)
        {
            foreach (var photo in request.Photos)
            {
                var resultImage = await _manageImageService.UploadImage(new ImageData
                {
                    ImageStream = photo.OpenReadStream(),
                    Name = photo.Name
                });

                var photoCommand = new CreateProductImageCommand
                {
                    PublicCode = resultImage.PublicId,
                    Url = resultImage.Url
                };

                listPhotoUrls.Add(photoCommand);
            }

            request.ImageUrls = listPhotoUrls;
        }

        return await _mediator.Send(request);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpDelete("status/{id}", Name = "UpdateStatusProduct")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductViewModel>> UpdateStatusProduct(int id)
    {
        var request = new DeleteProductCommand(id);

        return await _mediator.Send(request);
    }

}