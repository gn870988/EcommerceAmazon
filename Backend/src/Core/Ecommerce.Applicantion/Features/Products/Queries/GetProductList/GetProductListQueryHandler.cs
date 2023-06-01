using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Products.Queries.GetProductList;

public class GetProductListQueryHandler
    : IRequestHandler<GetProductListQuery, IReadOnlyList<Product>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<Product>> Handle(
        GetProductListQuery request,
        CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<Product, object>>>
        {
            p => p.Images!,
            p => p.Reviews!
        };

        var products = await _unitOfWork.Repository<Product>().GetAsync(
            null,
            x => x.OrderBy(y => y.Name),
            includes
        );

        return products;
    }
}