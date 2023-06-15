using Ecommerce.Application.Features.Orders.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.Payments.Commands.CreatePayment;

public class CreatePaymentCommand : IRequest<OrderViewModel>
{
    public int OrderId { get; set; }
    public Guid? ShoppingCartMasterId { get; set; }
}