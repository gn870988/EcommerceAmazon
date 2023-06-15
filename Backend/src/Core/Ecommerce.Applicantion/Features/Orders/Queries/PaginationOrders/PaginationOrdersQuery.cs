using Ecommerce.Application.Features.Orders.ViewModels;
using Ecommerce.Application.Features.Shared.Queries;
using MediatR;

namespace Ecommerce.Application.Features.Orders.Queries.PaginationOrders;

public class PaginationOrdersQuery : PaginationBaseQuery, IRequest<PaginationViewModel<OrderViewModel>>
{
    public int? Id { get; set; }
    public string? Username { get; set; }
}