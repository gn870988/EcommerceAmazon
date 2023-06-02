using Ecommerce.Application.Features.Images.Queries.ViewModels;
using Ecommerce.Application.Features.Reviews.Queries.ViewModels;
using Ecommerce.Application.Models.Product;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Products.Queries.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public int Rating { get; set; }

    public string? Seller { get; set; }

    public int Stock { get; set; }

    public virtual ICollection<ReviewViewModel>? Reviews { get; set; }

    public virtual ICollection<ImageViewModel>? Images { get; set; }

    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int NumberReviews { get; set; }

    public ProductStatus Status { get; set; }

    public string StatusLabel
    {
        get
        {
            return Status switch
            {
                ProductStatus.Active => ProductStatusLabel.Active,
                ProductStatus.Inactive => ProductStatusLabel.Inactive,
                _ => ProductStatusLabel.Inactive
            };
        }
    }
}