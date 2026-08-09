using Microsoft.Extensions.DependencyInjection;
using RSAM.Application.Auth.Interfaces;
using RSAM.Infrastructure.Auth.Services;

namespace RSAM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Add infrastructure services here
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
}
