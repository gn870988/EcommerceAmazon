using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Entities;

public class ShoppingCart : BaseDomainModel
{

    public Guid? ShoppingCartMasterId { get; set; }

    public virtual ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
}