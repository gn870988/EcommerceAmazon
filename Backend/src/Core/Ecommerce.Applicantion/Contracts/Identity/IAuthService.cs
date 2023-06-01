using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Contracts.Identity;

public interface IAuthService
{
    string GetSessionUser();
    string CreateToken(User user, IList<string>? roles);
}