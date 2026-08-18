using Asp.Versioning;
using Microsoft.OpenApi;
using RSAM.Api.Helpers;
using RSAM.Api.Swagger;
using RSAM.Application.Helpers;
using RSAM.Contracts.Constants;
using System.Reflection;

namespace RSAM.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });
        services.AddSwagger();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.1);

            options.AssumeDefaultVersionWhenUnspecified = true;

            options.ReportApiVersions = true;

            options.ApiVersionReader =
                new UrlSegmentApiVersionReader();
        }
        ).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return services;
    }
    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer", document), []
                }
            });
        });

        return services;
    }
}
