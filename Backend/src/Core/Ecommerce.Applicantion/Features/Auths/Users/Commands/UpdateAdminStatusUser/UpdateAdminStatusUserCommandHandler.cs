﻿using Ecommerce.Application.Exceptions;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminStatusUser;

public class UpdateAdminStatusUserCommandHandler : IRequestHandler<UpdateAdminStatusUserCommand, User>
{
    private readonly UserManager<User> _userManager;

    public UpdateAdminStatusUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> Handle(UpdateAdminStatusUserCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByIdAsync(request.Id!);
        if (updateUser is null)
        {
            throw new BadRequestException("The user does not exist");
        }

        updateUser.IsActive = !updateUser.IsActive;

        var result = await _userManager.UpdateAsync(updateUser);

        if (!result.Succeeded)
        {
            throw new Exception("Could not change user state");
        }

        return updateUser;
    }
}