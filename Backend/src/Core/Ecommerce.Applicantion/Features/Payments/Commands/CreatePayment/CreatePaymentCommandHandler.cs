using AutoMapper;
using Ecommerce.Application.Features.Orders.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, OrderViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderViewModel> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var orderToPay = await _unitOfWork.Repository<Order>().GetEntityAsync(
            x => x.Id == request.OrderId,
            null,
            false
        );

        orderToPay.Status = OrderStatus.Finished;
        _unitOfWork.Repository<Order>().UpdateEntity(orderToPay);

        var shoppingCartItems = await _unitOfWork.Repository<ShoppingCartItem>().GetAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartMasterId
        );

        _unitOfWork.Repository<ShoppingCartItem>().DeleteRange(shoppingCartItems);

        await _unitOfWork.Complete();

        return _mapper.Map<OrderViewModel>(orderToPay);
    }
}