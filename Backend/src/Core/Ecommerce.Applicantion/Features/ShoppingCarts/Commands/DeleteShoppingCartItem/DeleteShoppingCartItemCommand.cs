using Ecommerce.Application.Features.ShoppingCarts.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.DeleteShoppingCartItem;

public class DeleteShoppingCartItemCommand : IRequest<ShoppingCartViewModel>
{
    public int Id { get; set; }
}