using Ecommerce.Application.Features.Reviews.Queries.ViewModels;
using Ecommerce.Application.Features.Shared.Queries;
using MediatR;

namespace Ecommerce.Application.Features.Reviews.Queries.PaginationReviews;

public class PaginationReviewsQuery
    : PaginationBaseQuery, IRequest<PaginationViewModel<ReviewViewModel>>
{
    public int? ProductId { get; set; }
}