using Ecommerce.Application.Features.Addresses.ViewModels;
using Ecommerce.Application.Models.Order;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Orders.ViewModels;

public class OrderViewModel
{
    public int Id { get; set; }

    public AddressViewModel? OrderAddress { get; set; }

    public List<OrderItemViewModel>? OrderItems { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Tax { get; set; }

    public decimal Total { get; set; }

    public decimal ShippingPrice { get; set; }

    public OrderStatus Status { get; set; }

    public string? PaymentIntentId { get; set; }

    public string? ClientSecret { get; set; }

    public string? StripeApiKey { get; set; }

    public string? BuyerUsername { get; set; }

    public string? BuyerName { get; set; }

    public int Quantity => OrderItems!.Sum(x => x.Quantity);

    public string StatusLabel => Status switch
    {
        OrderStatus.Finished => OrderStatusLabel.COMPLETED,
        OrderStatus.Pending => OrderStatusLabel.PENDING,
        OrderStatus.Sent => OrderStatusLabel.SENT,
        OrderStatus.Mistake => OrderStatusLabel.ERROR,
        _ => OrderStatusLabel.ERROR
    };
}