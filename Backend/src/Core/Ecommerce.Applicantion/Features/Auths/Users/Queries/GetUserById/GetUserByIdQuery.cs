using Ecommerce.Application.Features.Auths.Users.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<AuthResponse>
{
    public GetUserByIdQuery(string userId)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
    }

    public string? UserId { get; set; }
}