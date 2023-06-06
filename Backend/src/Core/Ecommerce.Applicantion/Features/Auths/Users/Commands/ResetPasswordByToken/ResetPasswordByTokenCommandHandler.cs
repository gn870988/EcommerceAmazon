using Ecommerce.Application.Exceptions;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswordByToken;

public class ResetPasswordByTokenCommandHandler : IRequestHandler<ResetPasswordByTokenCommand, string>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordByTokenCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(ResetPasswordByTokenCommand request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.Password, request.ConfirmPassword))
        {
            throw new BadRequestException("The password does not equal the confirmation password");
        }

        var updateUser = await _userManager.FindByEmailAsync(request.Email!);
        if (updateUser is null)
        {
            throw new BadRequestException("The email is not registered as a user");
        }

        var resetResult = await _userManager.ResetPasswordAsync(
            updateUser,
            request.Token!,
            request.Password!);

        if (!resetResult.Succeeded)
        {
            throw new Exception("Could not reset password");
        }

        return $"Your password was successfully updated ${request.Email}";
    }
}