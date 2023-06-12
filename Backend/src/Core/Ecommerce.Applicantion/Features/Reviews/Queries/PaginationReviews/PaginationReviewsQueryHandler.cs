using AutoMapper;
using Ecommerce.Application.Features.Reviews.Queries.ViewModels;
using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specifications.Reviews;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Reviews.Queries.PaginationReviews;

public class PaginationReviewsQueryHandler : IRequestHandler<PaginationReviewsQuery, PaginationViewModel<ReviewViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaginationReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationViewModel<ReviewViewModel>> Handle(PaginationReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviewSpecificationParams = new ReviewSpecificationParams
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            Search = request.Search,
            Sort = request.Sort,
            ProductId = request.ProductId
        };

        var spec = new ReviewSpecification(reviewSpecificationParams);
        var reviews = await _unitOfWork.Repository<Review>().GetAllWithSpec(spec);

        var specCount = new ReviewForCountingSpecification(reviewSpecificationParams);
        var totalReviews = await _unitOfWork.Repository<Review>().CountAsync(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalReviews) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var data = _mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewViewModel>>(reviews);

        var reviewsByPage = reviews.Count;

        var pagination = new PaginationViewModel<ReviewViewModel>
        {
            Count = totalReviews,
            Data = data,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            ResultByPage = reviewsByPage
        };

        return pagination;
    }
}