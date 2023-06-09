using AutoMapper;
using Ecommerce.Application.Features.Countries.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Countries.Queries.GetCountryList;

public class GetCountryListQueryHandler : IRequestHandler<GetCountryListQuery, IReadOnlyList<CountryViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCountryListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CountryViewModel>> Handle(GetCountryListQuery request, CancellationToken cancellationToken)
    {
        var countries = await _unitOfWork.Repository<Country>().GetAsync(
            null,
            x => x.OrderBy(y => y.Name),
            string.Empty,
            false
        );

        return _mapper.Map<IReadOnlyList<CountryViewModel>>(countries);
    }
}