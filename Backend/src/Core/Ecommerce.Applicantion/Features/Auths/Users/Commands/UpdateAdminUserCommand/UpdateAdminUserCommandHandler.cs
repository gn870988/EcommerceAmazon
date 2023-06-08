using Ecommerce.Application.Exceptions;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUserCommand;

public class UpdateAdminUserCommandHandler : IRequestHandler<UpdateAdminUserCommand, User>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UpdateAdminUserCommandHandler(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<User> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByIdAsync(request.Id!);
        if (updateUser is null)
        {
            throw new BadRequestException("The user does not exist");
        }

        updateUser.Name = request.Name;
        updateUser.Lastname = request.Lastname;
        updateUser.Phone = request.Phone;

        var result = await _userManager.UpdateAsync(updateUser);

        if (!result.Succeeded)
        {
            throw new Exception("Could not update user");
        }

        var role = await _roleManager.FindByNameAsync(request.Role!);
        if (role is null)
        {
            throw new Exception("Assigned Role does not exist");
        }

        await _userManager.AddToRoleAsync(updateUser, role.Name!);

        return updateUser;
    }
}