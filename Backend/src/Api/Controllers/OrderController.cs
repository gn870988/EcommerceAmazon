using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.Commands.CreateAddress;
using Ecommerce.Application.Features.Addresses.ViewModels;
using Ecommerce.Application.Features.Orders.Commands.CreateOrder;
using Ecommerce.Application.Features.Orders.Commands.UpdateOrder;
using Ecommerce.Application.Features.Orders.Queries.GetOrdersById;
using Ecommerce.Application.Features.Orders.Queries.PaginationOrders;
using Ecommerce.Application.Features.Orders.ViewModels;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Models.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;

    public OrderController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost("address", Name = "CreateAddress")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<AddressViewModel>> CreateAddress([FromBody] CreateAddressCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(Name = "CreateOrder")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderViewModel>> CreateOrder([FromBody] CreateOrderCommand request)
    {
        return await _mediator.Send(request);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpPut(Name = "UpdateOrder")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderViewModel>> UpdateOrder([FromBody] UpdateOrderCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("{id}", Name = "GetOrderById")]
    [ProducesResponseType(typeof(OrderViewModel), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderViewModel>> GetOrderById(int id)
    {
        var query = new GetOrdersByIdQuery(id);

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("paginationByUsername", Name = "PaginationOrderByUsername")]
    [ProducesResponseType(typeof(PaginationViewModel<OrderViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationViewModel<OrderViewModel>>> PaginationOrderByUsername
    (
        [FromQuery] PaginationOrdersQuery paginationOrdersParams
    )
    {
        paginationOrdersParams.Username = _authService.GetSessionUser();
        var pagination = await _mediator.Send(paginationOrdersParams);

        return Ok(pagination);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("paginationAdmin", Name = "PaginationOrder")]
    [ProducesResponseType(typeof(PaginationViewModel<OrderViewModel>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationViewModel<OrderViewModel>>> PaginationOrder
    (
        [FromQuery] PaginationOrdersQuery paginationOrdersParams
    )
    {
        var pagination = await _mediator.Send(paginationOrdersParams);

        return Ok(pagination);
    }

}