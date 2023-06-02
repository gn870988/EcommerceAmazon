using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Specifications.Products;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecificationParams productParams)
        : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name!.Contains(productParams.Search)
                                                        || x.Description!.Contains(productParams.Search)
            ) &&
            (!productParams.CategoryId.HasValue || x.CategoryId == productParams.CategoryId) &&
            (!productParams.MinPrice.HasValue || x.Price >= productParams.MinPrice) &&
            (!productParams.MaxPrice.HasValue || x.Price <= productParams.MaxPrice) &&
            (!productParams.Status.HasValue || x.Status == productParams.Status))
    {
        AddInclude(p => p.Reviews!);
        AddInclude(p => p.Images!);

        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            switch (productParams.Sort)
            {
                case "nameAsc":
                    AddOrderBy(p => p.Name!);
                    break;

                case "nameDesc":
                    AddOrderByDescending(p => p.Name!);
                    break;

                case "priceAsc":
                    AddOrderBy(p => p.Price!);
                    break;

                case "priceDesc":
                    AddOrderByDescending(p => p.Price!);
                    break;

                case "ratingAsc":
                    AddOrderBy(p => p.Rating!);
                    break;

                case "ratingDesc":
                    AddOrderByDescending(p => p.Rating!);
                    break;

                default:
                    AddOrderBy(p => p.CreatedDate!);
                    break;
            }
        }
        else
        {
            AddOrderByDescending(p => p.CreatedDate!);
        }
    }
}