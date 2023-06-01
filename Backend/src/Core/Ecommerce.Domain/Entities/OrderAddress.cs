using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Entities;

public class OrderAddress : BaseDomainModel
{
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Department { get; set; }
    public string? ZipCode { get; set; }
    public string? Username { get; set; }
    public string? Country { get; set; }
}