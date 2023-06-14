using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Ecommerce.Application.Models.ImageManagement;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Models.Token;
using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.MessageImplementation;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Persistence;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection @this,
        IConfiguration configuration
    )
    {
        @this.AddScoped<IUnitOfWork, UnitOfWork>();
        @this.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

        @this.AddTransient<IEmailService, EmailService>();
        @this.AddTransient<IAuthService, AuthService>();

        @this.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        @this.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
        @this.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        @this.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

        return @this;
    }
}