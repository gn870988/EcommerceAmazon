namespace Ecommerce.Application.Features.ShoppingCarts.ViewModels;

public class ShoppingCartViewModel
{
    public string? ShoppingCartId { get; set; }
    public List<ShoppingCartItemViewModel>? ShoppingCartItems { get; set; }

    public decimal Total =>
        Math.Round(ShoppingCartItems!.Sum(x => x.Price * x.Quantity) +
                   ShoppingCartItems!.Sum(x => x.Price * x.Quantity) * Convert.ToDecimal(0.18) +
                   (ShoppingCartItems!.Sum(x => x.Price * x.Quantity) < 100 ? 10 : 25), 2);

    public int Quantity => ShoppingCartItems!.Sum(x => x.Quantity);

    public decimal SubTotal => Math.Round(ShoppingCartItems!.Sum(x => x.Price * x.Quantity), 2);

    public decimal Tax =>
        Math.Round((ShoppingCartItems!.Sum(x => x.Price * x.Quantity) * Convert.ToDecimal(0.18)), 2);

    public decimal ShippingPrice => ShoppingCartItems!.Sum(x => x.Price * x.Quantity);
}