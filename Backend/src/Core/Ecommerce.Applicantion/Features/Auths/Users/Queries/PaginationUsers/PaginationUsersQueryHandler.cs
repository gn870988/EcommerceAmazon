using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specifications.Users;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Queries.PaginationUsers;

public class PaginationUsersQueryHandler : IRequestHandler<PaginationUsersQuery, PaginationViewModel<User>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PaginationUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<PaginationViewModel<User>> Handle(PaginationUsersQuery request, CancellationToken cancellationToken)
    {
        var userSpecificationParams = new UserSpecificationParams
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            Search = request.Search,
            Sort = request.Sort
        };

        var spec = new UserSpecification(userSpecificationParams);
        var users = await _unitOfWork.Repository<User>().GetAllWithSpec(spec);

        var specCount = new UserForCountingSpecification(userSpecificationParams);
        var totalUsers = await _unitOfWork.Repository<User>().CountAsync(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalUsers) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var pagination = new PaginationViewModel<User>
        {
            Count = totalUsers,
            Data = users,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            ResultByPage = users.Count
        };

        return pagination;
    }
}