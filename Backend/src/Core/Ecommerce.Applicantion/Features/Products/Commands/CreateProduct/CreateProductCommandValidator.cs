using FluentValidation;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name cannot be blank")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description cannot be null");


        RuleFor(p => p.Stock)
            .NotEmpty().WithMessage("Stock cannot be null");

        RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Price cannot be null");
    }
}