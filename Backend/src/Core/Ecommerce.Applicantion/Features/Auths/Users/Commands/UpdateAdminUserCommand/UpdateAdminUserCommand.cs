using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUserCommand;

public class UpdateAdminUserCommand : IRequest<User>
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
}