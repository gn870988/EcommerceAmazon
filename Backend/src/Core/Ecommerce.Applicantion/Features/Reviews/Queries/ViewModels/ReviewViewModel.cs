namespace Ecommerce.Application.Features.Reviews.Queries.ViewModels;

public class ReviewViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public int ProductId { get; set; }
}