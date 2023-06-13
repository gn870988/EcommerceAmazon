namespace Ecommerce.Application.Features.ShoppingCarts.ViewModels;

public class ShoppingCartItemViewModel
{
    public int Id { get; set; }
    public string? Product { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Category { get; set; }
    public int Stock { get; set; }

    public decimal TotalLine => Math.Round(Quantity * Price, 2);
}