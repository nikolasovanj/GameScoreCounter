using Backend.Application.Interfaces.Services;
using Backend.Application.Scoring;
using Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IScoringRule, CatanScoring>();
        services.AddScoped<IScoringRule, SeaSaltScoring>();
        services.AddScoped<IScoringRule, WingspanScoring>();

        services.AddScoped<IMatchService, MatchService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
 
        return services;
    }
}