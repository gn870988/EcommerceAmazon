using Ecommerce.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain.Entities;

public class OrderItem : BaseDomainModel
{
    public Product? Product { get; set; }

    public int ProductId { get; set; }

    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public Order? Command { get; set; }

    public int OrderId { get; set; }

    public int ProductItemId { get; set; }

    public string? ProductName { get; set; }

    public string? ImageUrl { get; set; }
}