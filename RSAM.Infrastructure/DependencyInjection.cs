using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSAM.Application.Auth.Interfaces;
using RSAM.Application.Events;
using RSAM.Application.File;
using RSAM.Application.Helpers;
using RSAM.Application.Repositories;
using RSAM.Application.Time;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;
using RSAM.Infrastructure.Auth;
using RSAM.Infrastructure.Auth.Services;
using RSAM.Infrastructure.Context;
using RSAM.Infrastructure.Events;
using RSAM.Infrastructure.File;
using RSAM.Infrastructure.Repositories;
using RSAM.Infrastructure.Time;
using System.Reflection;

namespace RSAM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Inject DbContext
        services.AddDbContext<RSAMDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(RSAMDbContext.DatabaseName), 
                b => b.MigrationsHistoryTable("__RSAM_Migrations")));

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IFileStorage, FileStorage>();
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        AddRepositories(services);
        services.AddTransient<IOtpGeneator, OtpGenerator>();
        return services;
    }
    
    private static IServiceCollection AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IWriteRepository<User, UserId>, WriteUserRepository>();
        services.AddScoped<IWriteUserRepositry, WriteUserRepository>();
        services.AddScoped(typeof(IWriteRepository<,>), typeof(WriteRepository<,>));
        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped<IWriteUnitOfWork, WriteUnitOfWork>();
        services.AddScoped<IReadUnitOfWork, ReadUnitOfWork>();
        return services;
    }
}
