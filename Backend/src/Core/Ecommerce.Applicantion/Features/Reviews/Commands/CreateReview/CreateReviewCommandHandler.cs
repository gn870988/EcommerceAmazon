using AutoMapper;
using Ecommerce.Application.Features.Reviews.Queries.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewViewModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewViewModel> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewEntity = new Review
        {
            Comment = request.Comment,
            Rating = request.Rating,
            Name = request.Name,
            ProductId = request.ProductId
        };

        _unitOfWork.Repository<Review>().AddEntity(reviewEntity);
        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("Could not save comment");
        }

        return _mapper.Map<ReviewViewModel>(reviewEntity);
    }
}