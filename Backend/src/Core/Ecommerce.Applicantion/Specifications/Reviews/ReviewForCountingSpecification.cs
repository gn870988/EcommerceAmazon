using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Specifications.Reviews;

public class ReviewForCountingSpecification : BaseSpecification<Review>
{
    public ReviewForCountingSpecification(ReviewSpecificationParams reviewParams)
        : base(x =>
                !reviewParams.ProductId.HasValue ||
                x.ProductId == reviewParams.ProductId)
    {

    }
}