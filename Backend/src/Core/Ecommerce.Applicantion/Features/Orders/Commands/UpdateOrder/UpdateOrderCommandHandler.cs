using AutoMapper;
using Ecommerce.Application.Features.Orders.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderViewModel> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Repository<Order>().GetByIdAsync(request.OrderId);
        order.Status = request.Status;

        _unitOfWork.Repository<Order>().UpdateEntity(order);
        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("Could not update purchase order status");
        }

        return _mapper.Map<OrderViewModel>(order);
    }
}