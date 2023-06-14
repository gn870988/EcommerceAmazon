using AutoMapper;
using Ecommerce.Application.Features.Orders.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Orders.Queries.GetOrdersById;

public class GetOrdersByIdQueryHandler : IRequestHandler<GetOrdersByIdQuery, OrderViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrdersByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderViewModel> Handle(GetOrdersByIdQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<Order, object>>>
        {
            p => p.OrderItems!.OrderBy(x => x.ProductId),
            p => p.OrderAddress!
        };

        var order = await _unitOfWork.Repository<Order>().GetEntityAsync(
            x => x.Id == request.OrderId,
            includes,
            false
        );

        return _mapper.Map<OrderViewModel>(order);
    }
}