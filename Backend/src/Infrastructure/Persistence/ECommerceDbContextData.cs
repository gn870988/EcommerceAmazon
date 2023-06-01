using Ecommerce.Application.Models.Authorization;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ecommerce.Infrastructure.Persistence;

public class ECommerceDbContextData
{
    public static async Task LoadDataAsync(
        ECommerceDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        ILoggerFactory loggerFactory
    )
    {
        try
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Role.ADMIN));
                await roleManager.CreateAsync(new IdentityRole(Role.USER));
            }

            if (!userManager.Users.Any())
            {
                var userAdmin = new User
                {
                    Name = "Vincent",
                    Lastname = "YU",
                    Email = "gn870988@gmail.com",
                    UserName = "Vincent",
                    Phone = "985644644",
                    AvatarUrl =
                        "https://avatars.githubusercontent.com/u/9677189?s=400&u=f891178ce404c69bce21b48e5ca8882e355300e6&v=4",
                };

                await userManager.CreateAsync(userAdmin, "$Password123");
                await userManager.AddToRoleAsync(userAdmin, Role.ADMIN);

                var user = new User
                {
                    Name = "Jasmine",
                    Lastname = "Lin",
                    Email = "lcjasmine210@gmail.com",
                    UserName = "Jasmine",
                    Phone = "985644644",
                    AvatarUrl =
                        "https://avatars.githubusercontent.com/u/9677189?s=400&u=f891178ce404c69bce21b48e5ca8882e355300e6&v=4",
                };

                await userManager.CreateAsync(user, "$Password123");
                await userManager.AddToRoleAsync(user, Role.USER);
            }

            if (!context.Categories!.Any())
            {
                var categoryData = await File.ReadAllTextAsync("../Infrastructure/Data/category.json");
                var categories = JsonConvert.DeserializeObject<List<Category>>(categoryData);
                await context.Categories!.AddRangeAsync(categories!);
                await context.SaveChangesAsync();
            }

            if (!context.Products!.Any())
            {
                var productData = await File.ReadAllTextAsync("../Infrastructure/Data/product.json");
                var products = JsonConvert.DeserializeObject<List<Product>>(productData);
                await context.Products!.AddRangeAsync(products!);
                await context.SaveChangesAsync();
            }

            if (!context.Images!.Any())
            {
                var imageData = await File.ReadAllTextAsync("../Infrastructure/Data/image.json");
                var images = JsonConvert.DeserializeObject<List<Image>>(imageData);
                await context.Images!.AddRangeAsync(images!);
                await context.SaveChangesAsync();
            }

            if (!context.Reviews!.Any())
            {
                var reviewData = await File.ReadAllTextAsync("../Infrastructure/Data/review.json");
                var reviews = JsonConvert.DeserializeObject<List<Review>>(reviewData);
                await context.Reviews!.AddRangeAsync(reviews!);
                await context.SaveChangesAsync();
            }

            if (!context.Countries!.Any())
            {
                var countryData = await File.ReadAllTextAsync("../Infrastructure/Data/country.json");
                var countries = JsonConvert.DeserializeObject<List<Country>>(countryData);
                await context.Countries!.AddRangeAsync(countries!);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<ECommerceDbContextData>();
            logger.LogError(e.Message);
        }

    }
}