using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Addresses.ViewModels;
using Ecommerce.Application.Features.Auths.Users.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Address = Ecommerce.Domain.Entities.Address;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _sigInManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserCommandHandler(
        UserManager<User> userManager,
        SignInManager<User> sigInManager,
        IAuthService authService,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _sigInManager = sigInManager;
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);

        if (user is null)
        {
            throw new NotFoundException(nameof(User), request.Email!);
        }

        if (!user.IsActive)
        {
            throw new Exception("The user is blocked, contact the admin");
        }

        var result = await _sigInManager.CheckPasswordSignInAsync(user, request.Password!, false);

        if (!result.Succeeded)
        {
            throw new Exception("The user credentials are wrong");
        }

        var shippingAddress = await _unitOfWork.Repository<Address>().GetEntityAsync(
            x => x.Username == user.UserName
        );

        var roles = await _userManager.GetRolesAsync(user);

        var response = new AuthResponse
        {
            Id = user.Id,
            Name = user.Name,
            Lastname = user.Lastname,
            Phone = user.Phone,
            Email = user.Email,
            Username = user.UserName,
            Avatar = user.AvatarUrl,
            ShippingAddress = _mapper.Map<AddressViewModel>(shippingAddress),
            Token = _authService.CreateToken(user, roles),
            Roles = roles
        };

        return response;
    }
}