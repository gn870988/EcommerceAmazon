using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.ViewModels;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(
        UserManager<User> userManager,
        IAuthService authService
    )
    {
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existsUserByEmail = await _userManager.FindByEmailAsync(request.Email!) != null;
        if (existsUserByEmail)
        {
            throw new BadRequestException("The user's email already exists in the database");
        }

        var existsUserByUsername = await _userManager.FindByNameAsync(request.Username!) != null;
        if (existsUserByUsername)
        {
            throw new BadRequestException("User's Username already exists in the database");
        }

        var user = new User
        {
            Name = request.Name,
            Lastname = request.Lastname,
            Phone = request.Phone,
            Email = request.Email,
            UserName = request.Username,
            AvatarUrl = request.PhotoUrl
        };

        var result = await _userManager.CreateAsync(user, request.Password!);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, AppRole.GenericUser);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponse
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Phone = user.Phone,
                Email = user.Email,
                Username = user.UserName,
                Avatar = user.AvatarUrl,
                Token = _authService.CreateToken(user, roles),
                Roles = roles
            };
        }

        throw new Exception("Failed to register user");
    }
}