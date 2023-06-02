using Ecommerce.Application.Features.Products.Queries.ViewModels;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.PaginationProducts;

public class PaginationProductsQuery
    : PaginationBaseQuery, IRequest<PaginationViewModel<ProductViewModel>>
{
    public int? CategoryId { get; set; }

    public decimal? MaxPrice { get; set; }

    public decimal? MinPrice { get; set; }

    public int? Rating { get; set; }

    public ProductStatus? Status { get; set; }
}