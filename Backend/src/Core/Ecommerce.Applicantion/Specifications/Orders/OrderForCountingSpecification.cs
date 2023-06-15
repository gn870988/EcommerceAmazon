using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Specifications.Orders;

public class OrderForCountingSpecification : BaseSpecification<Order>
{
    public OrderForCountingSpecification(OrderSpecificationParams orderParams)
        : base(x => (string.IsNullOrEmpty(orderParams.Username) ||
                     x.BuyerUsername!.Contains(orderParams.Username)) &&
                    (!orderParams.Id.HasValue || x.Id == orderParams.Id))
    {

    }
}