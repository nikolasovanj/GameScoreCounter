using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Options;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default"))
                   .UseSnakeCaseNamingConvention());

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();

        services.Configure<PasswordHasherOptions>(o => o.IterationCount = 210_000);
        services.AddSingleton<IPasswordService, PasswordService>();
        
        services.Configure<JwtOptions>(config.GetSection("Jwt"));
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
