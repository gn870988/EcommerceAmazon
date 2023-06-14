using Ecommerce.Application.Features.Orders.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<OrderViewModel>
{
    public Guid? ShoppingCartId { get; set; }
}