using Ecommerce.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain.Entities;

public class Category : BaseDomainModel
{
    [Column(TypeName = "NVARCHAR(100)")]
    public string? Name { get; set; }

    public virtual ICollection<Product>? Products { get; set; }
}