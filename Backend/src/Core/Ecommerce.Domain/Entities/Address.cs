using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Entities;

public class Address : BaseDomainModel
{
    public string? HomeAddress { get; set; }

    public string? City { get; set; }

    public string? Department { get; set; }

    public string? ZipCode { get; set; }

    public string? Username { get; set; }

    public string? Country { get; set; }
}