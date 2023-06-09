using Ecommerce.Application.Features.Categories.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.Categories;

public class GetCategoryListQuery : IRequest<IReadOnlyList<CategoryViewModel>>
{

}