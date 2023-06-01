using Ecommerce.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain.Entities;

public class ShoppingCartItem : BaseDomainModel
{
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? Image { get; set; }

    public string? Category { get; set; }

    public Guid? ShoppingCartMasterId { get; set; }

    public int ShoppingCartId { get; set; }

    public virtual ShoppingCart? ShoppingCart { get; set; }

    public int ProductId { get; set; }

    public int Stock { get; set; }
}