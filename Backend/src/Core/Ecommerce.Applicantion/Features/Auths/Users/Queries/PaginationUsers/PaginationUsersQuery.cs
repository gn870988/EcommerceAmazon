using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Queries.PaginationUsers;

public class PaginationUsersQuery : PaginationBaseQuery, IRequest<PaginationViewModel<User>>
{

}