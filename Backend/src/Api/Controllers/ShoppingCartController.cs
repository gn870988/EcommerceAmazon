using Ecommerce.Application.Features.ShoppingCarts.Commands.DeleteShoppingCartItem;
using Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart;
using Ecommerce.Application.Features.ShoppingCarts.Queries.GetShoppingCartById;
using Ecommerce.Application.Features.ShoppingCarts.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private IMediator _mediator;

    public ShoppingCartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("{id}", Name = "GetShoppingCart")]
    [ProducesResponseType(typeof(ShoppingCartViewModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartViewModel>> GetShoppingCart(Guid id)
    {
        var shoppingCartId = id == Guid.Empty ? Guid.NewGuid() : id;
        var query = new GetShoppingCartByIdQuery(shoppingCartId);

        return await _mediator.Send(query);
    }

    [AllowAnonymous]
    [HttpPut("{id}", Name = "UpdateShoppingCart")]
    [ProducesResponseType(typeof(ShoppingCartViewModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartViewModel>> UpdateShoppingCart(Guid id, UpdateShoppingCartCommand request)
    {
        request.ShoppingCartId = id;

        return await _mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpDelete("item/{id}", Name = "DeleteShoppingCart")]
    [ProducesResponseType(typeof(ShoppingCartViewModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartViewModel>> DeleteShoppingCart(int id)
    {
        return await _mediator.Send(new DeleteShoppingCartItemCommand { Id = id });
    }


}