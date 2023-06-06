using Ecommerce.Application.Features.Auths.Users.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<AuthResponse>
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public IFormFile? Photo { get; set; }
    public string? PhotoUrl { get; set; }
    public string? PhotoId { get; set; }
}