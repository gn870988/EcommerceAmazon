﻿using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Queries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<ProductViewModel>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Seller { get; set; }
    public int Stock { get; set; }
    public string? CategoryId { get; set; }
    public IReadOnlyList<IFormFile>? Photos { get; set; }
    public IReadOnlyList<CreateProductImageCommand>? ImageUrls { get; set; }
}