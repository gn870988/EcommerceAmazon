using Ecommerce.Application.Features.ShoppingCarts.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart;

public class UpdateShoppingCartCommand : IRequest<ShoppingCartViewModel>
{
    public Guid? ShoppingCartId { get; set; }
    public List<ShoppingCartItemViewModel>? ShoppingCartItems { get; set; }
}