using Ecommerce.Application.Features.Auths.Users.ViewModels;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQuery : IRequest<AuthResponse>
{
    public string? Username { get; set; }

    public GetUserByUsernameQuery(string username)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
    }
}