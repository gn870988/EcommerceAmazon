using FluentValidation;

namespace Ecommerce.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(p => p.HomeAddress)
            .NotEmpty().WithMessage("Address cannot be null");

        RuleFor(p => p.City)
            .NotEmpty().WithMessage("City cannot be null");

        RuleFor(p => p.Department)
            .NotEmpty().WithMessage("Department cannot be null");

        RuleFor(p => p.ZipCode)
            .NotEmpty().WithMessage("Postal Code cannot be null");

        RuleFor(p => p.Country)
            .NotEmpty().WithMessage("Country cannot be null");
    }
}