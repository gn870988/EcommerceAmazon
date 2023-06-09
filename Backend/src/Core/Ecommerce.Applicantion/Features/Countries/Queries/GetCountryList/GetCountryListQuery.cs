using Ecommerce.Application.Features.Countries.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.Countries.Queries.GetCountryList;

public class GetCountryListQuery : IRequest<IReadOnlyList<CountryViewModel>>
{

}