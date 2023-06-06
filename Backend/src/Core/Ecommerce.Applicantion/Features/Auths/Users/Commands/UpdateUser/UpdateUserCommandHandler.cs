using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.ViewModels;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public UpdateUserCommandHandler(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IAuthService authService
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if (updateUser is null)
        {
            throw new BadRequestException("The user does not exist");
        }

        updateUser.Name = request.Name;
        updateUser.Lastname = request.Lastname;
        updateUser.Phone = request.Phone;
        updateUser.AvatarUrl = request.PhotoUrl ?? updateUser.AvatarUrl;

        var result = await _userManager.UpdateAsync(updateUser);

        if (!result.Succeeded)
        {
            throw new Exception("Could not update user");
        }

        var userById = await _userManager.FindByEmailAsync(request.Email!);
        var roles = await _userManager.GetRolesAsync(userById!);

        return new AuthResponse
        {
            Id = userById!.Id,
            Name = userById.Name,
            Lastname = userById.Lastname,
            Phone = userById.Phone,
            Email = userById.Email,
            Username = userById.UserName,
            Avatar = userById.AvatarUrl,
            Token = _authService.CreateToken(userById, roles),
            Roles = roles
        };
    }
}