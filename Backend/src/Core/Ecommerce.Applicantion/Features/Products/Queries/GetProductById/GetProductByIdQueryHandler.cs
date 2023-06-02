using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductViewModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<Product, object>>>
        {
            p => p.Images!,
            p => p.Reviews!.OrderByDescending(x => x.CreatedDate)
        };

        var product = await _unitOfWork.Repository<Product>().GetEntityAsync(
            x => x.Id == request.ProductId,
            includes
        );

        return _mapper.Map<ProductViewModel>(product);
    }
}
