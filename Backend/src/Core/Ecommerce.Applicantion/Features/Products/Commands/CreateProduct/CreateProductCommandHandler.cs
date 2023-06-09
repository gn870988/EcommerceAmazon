using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.ViewModels;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductViewModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productEntity = _mapper.Map<Product>(request);
        await _unitOfWork.Repository<Product>().AddAsync(productEntity);

        if (request.ImageUrls is not null && request.ImageUrls.Count > 0)
        {
            var createProductImageCommands = request.ImageUrls
                .Select(c => { c.ProductId = productEntity.Id; return c; });

            request.ImageUrls = createProductImageCommands.ToList();

            var images = _mapper.Map<List<Image>>(request.ImageUrls);
            _unitOfWork.Repository<Image>().AddRange(images);

            await _unitOfWork.Complete();
        }

        return _mapper.Map<ProductViewModel>(productEntity);
    }
}