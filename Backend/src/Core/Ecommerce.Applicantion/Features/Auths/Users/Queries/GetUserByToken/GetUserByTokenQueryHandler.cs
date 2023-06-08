using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Features.Addresses.ViewModels;
using Ecommerce.Application.Features.Auths.Users.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByToken;

public class GetUserByTokenQueryHandler : IRequestHandler<GetUserByTokenQuery, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByTokenQueryHandler(
        UserManager<User> userManager,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _userManager = userManager;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(_authService.GetSessionUser());
        if (user is null)
        {
            throw new Exception("The user is not authenticated");
        }

        if (!user.IsActive)
        {
            throw new Exception("User is locked out, contact admin");
        }

        var shippingAddress = await _unitOfWork.Repository<Address>().GetEntityAsync(
            x => x.Username == user.UserName
        );

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
            ShippingAddress = _mapper.Map<AddressViewModel>(shippingAddress),
            Token = _authService.CreateToken(user, roles),
            Roles = roles
        };
    }
}