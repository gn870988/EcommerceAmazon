using AutoMapper;
using Ecommerce.Application.Features.Categories.ViewModels;
using Ecommerce.Application.Features.Countries.ViewModels;
using Ecommerce.Application.Features.Images.Queries.ViewModels;
using Ecommerce.Application.Features.Products.Queries.ViewModels;
using Ecommerce.Application.Features.Reviews.Queries.ViewModels;
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
    }
}