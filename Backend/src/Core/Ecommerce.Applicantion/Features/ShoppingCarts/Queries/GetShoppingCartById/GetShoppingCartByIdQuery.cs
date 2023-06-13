using Ecommerce.Application.Features.ShoppingCarts.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCarts.Queries.GetShoppingCartById;

public class GetShoppingCartByIdQuery : IRequest<ShoppingCartViewModel>
{
    public Guid? ShoppingCartId { get; set; }

    public GetShoppingCartByIdQuery(Guid? shoppingCartId)
    {
        ShoppingCartId = shoppingCartId ?? throw new ArgumentNullException(nameof(shoppingCartId));
    }
}