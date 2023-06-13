
using AutoMapper;
using Ecommerce.Application.Features.ShoppingCarts.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.ShoppingCarts.Queries.GetShoppingCartById;

public class GetShoppingCartByIdQueryHandler : IRequestHandler<GetShoppingCartByIdQuery, ShoppingCartViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetShoppingCartByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ShoppingCartViewModel> Handle(GetShoppingCartByIdQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<ShoppingCart, object>>>
        {
            p => p.ShoppingCartItems!.OrderBy(x => x.ProductId)
        };

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId,
            includes
        );

        if (shoppingCart == null)
        {
            shoppingCart = new ShoppingCart
            {
                ShoppingCartMasterId = request.ShoppingCartId,
                ShoppingCartItems = new List<ShoppingCartItem>()
            };

            _unitOfWork.Repository<ShoppingCart>().AddEntity(shoppingCart);

            await _unitOfWork.Complete();
        }

        return _mapper.Map<ShoppingCartViewModel>(shoppingCart);
    }
}