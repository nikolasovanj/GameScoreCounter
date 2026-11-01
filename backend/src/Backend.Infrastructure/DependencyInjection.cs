using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //   services.AddScoped<IScoringRule, ChessScoring>();
        //   services.AddScoped<IScoringRule, DartsScoring>();
        //   services.AddScoped<IScoringRule, RummyScoring>();
 
        //   services.AddScoped<IMatchService, MatchService>();
        //   services.AddScoped<IProductService, ProductService>();
        //   services.AddScoped<IProductImageService, ProductImageService>();
 
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default"))
                   .UseSnakeCaseNamingConvention());

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        return services;
    }
}
