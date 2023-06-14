using Ecommerce.Application.Features.Orders.ViewModels;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest<OrderViewModel>
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}