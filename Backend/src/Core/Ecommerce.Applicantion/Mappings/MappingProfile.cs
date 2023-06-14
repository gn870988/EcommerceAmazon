using AutoMapper;
using Ecommerce.Application.Features.Addresses.ViewModels;
using Ecommerce.Application.Features.Categories.ViewModels;
using Ecommerce.Application.Features.Countries.ViewModels;
using Ecommerce.Application.Features.Images.Queries.ViewModels;
using Ecommerce.Application.Features.Orders.ViewModels;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Queries.ViewModels;
using Ecommerce.Application.Features.Reviews.Queries.ViewModels;
using Ecommerce.Application.Features.ShoppingCarts.ViewModels;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductViewModel>()
            .ForMember(p => p.CategoryName, x => x.MapFrom(a => a.Category!.Name))
            .ForMember(p => p.NumberReviews, x => x.MapFrom(a => a.Reviews == null ? 0 : a.Reviews.Count));

        CreateMap<Image, ImageViewModel>();
        CreateMap<Review, ReviewViewModel>();
        CreateMap<Country, CountryViewModel>();
        CreateMap<Category, CategoryViewModel>();
        CreateMap<CreateProductCommand, Product>();
        CreateMap<CreateProductImageCommand, Image>();
        CreateMap<UpdateProductCommand, Product>();

        CreateMap<ShoppingCart, ShoppingCartViewModel>()
            .ForMember(p => p.ShoppingCartId, x => x.MapFrom(a => a.ShoppingCartMasterId));
        CreateMap<ShoppingCartItem, ShoppingCartItemViewModel>();
        CreateMap<ShoppingCartItemViewModel, ShoppingCartItem>();

        CreateMap<Address, AddressViewModel>();

        CreateMap<Order, OrderViewModel>();
        CreateMap<OrderItem, OrderItemViewModel>();
        CreateMap<OrderAddress, AddressViewModel>();
    }
}