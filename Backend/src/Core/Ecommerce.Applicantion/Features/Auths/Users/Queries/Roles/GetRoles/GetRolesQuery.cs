using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Queries.Roles.GetRoles;

public class GetRolesQuery : IRequest<List<string>>
{

}