using AutoMapper;
using Ecommerce.Application.Features.ShoppingCarts.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.DeleteShoppingCartItem;

public class DeleteShoppingCartItemCommandHandler : IRequestHandler<DeleteShoppingCartItemCommand, ShoppingCartViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteShoppingCartItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ShoppingCartViewModel> Handle(DeleteShoppingCartItemCommand request, CancellationToken cancellationToken)
    {
        var shoppingCartItem = await _unitOfWork.Repository<ShoppingCartItem>().GetEntityAsync(
            x => x.Id == request.Id
        );

        await _unitOfWork.Repository<ShoppingCartItem>().DeleteAsync(shoppingCartItem);


        var includes = new List<Expression<Func<ShoppingCart, object>>>
        {
            p => p.ShoppingCartItems!.OrderBy(x => x.ProductId)
        };

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == shoppingCartItem.ShoppingCartMasterId,
            includes
        );

        return _mapper.Map<ShoppingCartViewModel>(shoppingCart);
    }
}