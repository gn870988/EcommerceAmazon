using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUserCommand;

public class UpdateAdminUserCommandValidator : AbstractValidator<UpdateAdminUserCommand>
{
    public UpdateAdminUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The name cannot be empty");

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Last name cannot be empty");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("The phone cannot be empty");
    }
}