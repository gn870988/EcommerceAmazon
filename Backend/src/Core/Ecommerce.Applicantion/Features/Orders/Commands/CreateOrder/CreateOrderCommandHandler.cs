using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Orders.ViewModels;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Stripe;
using System.Linq.Expressions;
using Address = Ecommerce.Domain.Entities.Address;

namespace Ecommerce.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly StripeSettings _stripeSettings;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuthService authService,
        UserManager<User> userManager,
        IOptions<StripeSettings> stripeSettings
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authService = authService;
        _userManager = userManager;
        _stripeSettings = stripeSettings.Value;
    }

    public async Task<OrderViewModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderPending = await _unitOfWork.Repository<Order>().GetEntityAsync(
              x => x.BuyerUsername == _authService.GetSessionUser() && x.Status == OrderStatus.Pending
          );

        if (orderPending is not null)
        {
            await _unitOfWork.Repository<Order>().DeleteAsync(orderPending);
        }

        var includes = new List<Expression<Func<ShoppingCart, object>>>
        {
            p => p.ShoppingCartItems!.OrderBy(x => x.ProductId)
        };

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == request.ShoppingCartId,
            includes,
            false
        );

        var user = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if (user is null)
        {
            throw new Exception("The user is not authenticated");
        }

        var address = await _unitOfWork.Repository<Address>().GetEntityAsync(
            x => x.Username == user.UserName,
            null,
            false
        );

        OrderAddress orderAddress = new()
        {
            Address = address.HomeAddress,
            City = address.City,
            ZipCode = address.ZipCode,
            Country = address.Country,
            Department = address.Department,
            Username = address.Username
        };

        await _unitOfWork.Repository<OrderAddress>().AddAsync(orderAddress);

        var subtotal = Math.Round(shoppingCart.ShoppingCartItems!.Sum(x => x.Price * x.Quantity), 2);
        var tax = Math.Round(subtotal * Convert.ToDecimal(0.18), 2);
        var shippingPrice = subtotal < 100 ? 10 : 25;
        var total = subtotal + tax + shippingPrice;

        var buyerName = $"{user.Name} {user.Lastname}";
        var order = new Order(buyerName, user.UserName!, orderAddress, subtotal, total, tax, shippingPrice);

        await _unitOfWork.Repository<Order>().AddAsync(order);

        var items = shoppingCart.ShoppingCartItems!.Select(shoppingElement => new OrderItem
        {
            ProductName = "NoneName",
            ProductId = shoppingElement.ProductId,
            ImageUrl = shoppingElement.Image,
            Price = shoppingElement.Price,
            Quantity = shoppingElement.Quantity,
            OrderId = order.Id
        }).ToList();

        _unitOfWork.Repository<OrderItem>().AddRange(items);

        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("Error creating purchase order");
        }

        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        var service = new PaymentIntentService();

        if (string.IsNullOrEmpty(order.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)order.Total,
                Currency = "USD",
                PaymentMethodTypes = new List<string> { "card" }
            };

            var intent = await service.CreateAsync(options, cancellationToken: cancellationToken);
            order.PaymentIntentId = intent.Id;
            order.ClientSecret = intent.ClientSecret;
            order.StripeApiKey = _stripeSettings.PublishableKey;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)order.Total
            };

            await service.UpdateAsync(order.PaymentIntentId, options, cancellationToken: cancellationToken);
        }

        _unitOfWork.Repository<Order>().UpdateEntity(order);
        var resultOrder = await _unitOfWork.Complete();

        if (resultOrder <= 0)
        {
            throw new Exception("Error creating payment intent in stripe");
        }

        return _mapper.Map<OrderViewModel>(order);
    }
}