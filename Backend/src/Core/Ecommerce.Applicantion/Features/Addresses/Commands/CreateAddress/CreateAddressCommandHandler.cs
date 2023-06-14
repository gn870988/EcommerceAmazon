using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, AddressViewModel>
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAddressCommandHandler(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AddressViewModel> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var addressRecord = await _unitOfWork.Repository<Address>().GetEntityAsync(
            x => x.Username == _authService.GetSessionUser(),
            null,
            false
        );

        if (addressRecord is null)
        {
            addressRecord = new Address
            {
                HomeAddress = request.HomeAddress,
                City = request.City,
                Department = request.Department,
                ZipCode = request.ZipCode,
                Country = request.Country,
                Username = _authService.GetSessionUser()
            };

            _unitOfWork.Repository<Address>().AddEntity(addressRecord);
        }
        else
        {
            addressRecord.HomeAddress = request.HomeAddress;
            addressRecord.City = request.City;
            addressRecord.Department = request.Department;
            addressRecord.ZipCode = request.ZipCode;
            addressRecord.Country = request.Country;
        }

        await _unitOfWork.Complete();

        return _mapper.Map<AddressViewModel>(addressRecord);
    }
}