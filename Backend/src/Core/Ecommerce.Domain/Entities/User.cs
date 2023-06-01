using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Domain.Entities;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; } = true;
}