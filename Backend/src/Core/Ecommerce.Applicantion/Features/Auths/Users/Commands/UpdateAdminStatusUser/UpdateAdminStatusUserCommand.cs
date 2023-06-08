using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminStatusUser;

public class UpdateAdminStatusUserCommand : IRequest<User>
{
    public string? Id { get; set; }
}