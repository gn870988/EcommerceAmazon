using Ecommerce.Api.Middlewares;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Registration;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.ImageCloudinary;
using Ecommerce.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddDbContext<ECommerceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"),
        b => b.MigrationsAssembly(typeof(ECommerceDbContext).Assembly.FullName));
});

builder.Services.AddMediatR(typeof(GetProductListQueryHandler).Assembly);
builder.Services.AddScoped<IManageImageService, ManageImageService>();

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    opt.Filters.Add(new AuthorizeFilter(policy));
}).AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<User>();
identityBuilder = new IdentityBuilder(identityBuilder.UserType, identityBuilder.Services);

identityBuilder.AddRoles<IdentityRole>().AddDefaultTokenProviders();
identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>();

identityBuilder.AddEntityFrameworkStores<ECommerceDbContext>();
identityBuilder.AddSignInManager<SignInManager<User>>();

builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = service.GetRequiredService<ECommerceDbContext>();
        var userManager = service.GetRequiredService<UserManager<User>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();
        await ECommerceDbContextData.LoadDataAsync(context, userManager, roleManager, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Migration failed");
    }
}

app.Run();
