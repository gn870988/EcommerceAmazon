using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.ShoppingCarts.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart;

public class UpdateShoppingCartCommandHandler : IRequestHandler<UpdateShoppingCartCommand, ShoppingCartViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateShoppingCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ShoppingCartViewModel> Handle(UpdateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var shoppingCartToUpdate = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            p => p.ShoppingCartMasterId == request.ShoppingCartId
        );

        if (shoppingCartToUpdate is null)
        {
            throw new NotFoundException(nameof(ShoppingCart), request.ShoppingCartId!);
        }

        var shoppingCartItems = await _unitOfWork.Repository<ShoppingCartItem>().GetAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId
        );

        _unitOfWork.Repository<ShoppingCartItem>().DeleteRange(shoppingCartItems);

        var shoppingCartItemsToAdd = _mapper.Map<List<ShoppingCartItem>>(request.ShoppingCartItems);
        shoppingCartItemsToAdd.ForEach(x =>
        {
            x.ShoppingCartId = shoppingCartToUpdate.Id;
            x.ShoppingCartMasterId = request.ShoppingCartId;
        });

        _unitOfWork.Repository<ShoppingCartItem>().AddRange(shoppingCartItemsToAdd);

        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("Could not add items to shopping cart");
        }

        var includes = new List<Expression<Func<ShoppingCart, object>>>
        {
            p => p.ShoppingCartItems!.OrderBy(x => x.ProductId)
        };

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId,
            includes
        );

        return _mapper.Map<ShoppingCartViewModel>(shoppingCart);
    }
}