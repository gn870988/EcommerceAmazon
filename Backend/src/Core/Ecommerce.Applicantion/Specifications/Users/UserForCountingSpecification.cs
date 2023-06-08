using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Specifications.Users;

public class UserForCountingSpecification : BaseSpecification<User>
{
    public UserForCountingSpecification(UserSpecificationParams userParams) : base(
        x =>
            (string.IsNullOrEmpty(userParams.Search) ||
             x.Name!.Contains(userParams.Search) ||
             x.Lastname!.Contains(userParams.Search) ||
             x.Email!.Contains(userParams.Search)
            )
    )
    {
    }
}