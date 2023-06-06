using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public ResetPasswordCommandHandler(UserManager<User> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if (updateUser is null)
        {
            throw new BadRequestException("The user does not exist");
        }

        var resultValidateOldPassword = _userManager.PasswordHasher
            .VerifyHashedPassword(updateUser, updateUser.PasswordHash!, request.OldPassword!);

        if (resultValidateOldPassword != PasswordVerificationResult.Success)
        {
            throw new BadRequestException("The current password entered is wrong");
        }

        var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updateUser, request.NewPassword!);
        updateUser.PasswordHash = hashedNewPassword;

        var result = await _userManager.UpdateAsync(updateUser);

        if (!result.Succeeded)
        {
            throw new Exception("Could not reset password");
        }

        return Unit.Value;
    }
}