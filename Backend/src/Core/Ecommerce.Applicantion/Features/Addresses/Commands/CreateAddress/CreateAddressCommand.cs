using Ecommerce.Application.Features.Addresses.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommand : IRequest<AddressViewModel>
{
    public string? HomeAddress { get; set; }
    public string? City { get; set; }
    public string? Department { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
}