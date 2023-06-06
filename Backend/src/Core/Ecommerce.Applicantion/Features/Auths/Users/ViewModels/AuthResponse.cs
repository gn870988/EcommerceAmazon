using Ecommerce.Application.Features.Addresses.ViewModels;

namespace Ecommerce.Application.Features.Auths.Users.ViewModels;

public class AuthResponse
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Lastname { get; set; }
    public string? Phone { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? Avatar { get; set; }
    public AddressViewModel? ShippingAddress { get; set; }
    public ICollection<string>? Roles { get; set; }
}