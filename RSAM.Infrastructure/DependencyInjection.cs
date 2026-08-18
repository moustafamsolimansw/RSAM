using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
using System.Text;

namespace RSAM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        // Inject DbContext
        services.AddDbContext<RSAMDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(RSAMDbContext.DatabaseName), 
                b => b.MigrationsHistoryTable("__RSAM_Migrations")));
        services.AddAuth(configuration);

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

    private static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration) 
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();
        var secret = !string.IsNullOrEmpty(jwtSettings.Secret) 
            ? jwtSettings.Secret 
            : "your_secret_key_here";
        var issuer = !string.IsNullOrEmpty(jwtSettings.Issuer) ? jwtSettings.Issuer : "RSAM";
        var audience = !string.IsNullOrEmpty(jwtSettings.Audience) ? jwtSettings.Audience : "RSAM";

        services.AddSingleton(Options.Create(jwtSettings));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = issuer,
                   ValidAudience = audience,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
               };

               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       var header = context.Request.Headers["Authorization"].ToString().Trim();
                       Console.WriteLine($"[JWT Auth] MessageReceived: Raw Authorization Header = '{header}'");

                       if (!string.IsNullOrWhiteSpace(header))
                       {
                           string token = header;
                           if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                           {
                               token = token.Substring(7).Trim();
                           }

                           // Handle duplicated token if pasted twice (e.g. eyJhbGci...eyJhbGci...)
                           if (token.Contains("eyJhbGci"))
                           {
                               var firstIndex = token.IndexOf("eyJhbGci");
                               var secondIndex = token.IndexOf("eyJhbGci", firstIndex + 1);
                               if (secondIndex > 0)
                               {
                                   token = token.Substring(firstIndex, secondIndex - firstIndex).Trim();
                               }
                           }

                           context.Token = token;
                           Console.WriteLine($"[JWT Auth] Extracted Token = '{context.Token[..Math.Min(25, context.Token.Length)]}...'");
                       }

                       return Task.CompletedTask;
                   },
                   OnTokenValidated = context =>
                   {
                       Console.WriteLine($"[JWT Auth] TokenValidated: Subject = '{context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value}'");
                       return Task.CompletedTask;
                   },
                   OnAuthenticationFailed = context =>
                   {
                       Console.WriteLine($"[JWT Auth Failed]: Exception Type = {context.Exception.GetType().Name}, Exception Message = {context.Exception.Message}");
                       return Task.CompletedTask;
                   },
                   OnChallenge = context =>
                   {
                       Console.WriteLine($"[JWT Auth Challenge]: Error = '{context.Error}', Description = '{context.ErrorDescription}'");
                       return Task.CompletedTask;
                   }
               };
           });
        return services;
    }
}
