using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name cannot be null");

        RuleFor(p => p.Lastname)
            .NotEmpty().WithMessage("Last name cannot be null");
    }
}