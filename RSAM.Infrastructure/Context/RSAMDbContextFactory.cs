using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RSAM.Infrastructure.Context;

public class RSAMDbContextFactory : IDesignTimeDbContextFactory<RSAMDbContext>
{
    public RSAMDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        var connectionString = configuration.GetConnectionString(RSAMDbContext.DatabaseName)
            ?? throw new InvalidOperationException($"Could not find a connection string named '{RSAMDbContext.DatabaseName}'.");

        var optionBuilder = new DbContextOptionsBuilder<RSAMDbContext>();
        optionBuilder.UseNpgsql(connectionString, b =>
        {
            b.MigrationsHistoryTable("__RSAM_Migrations");
        });
        
        return new RSAMDbContext(optionBuilder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "RSAM.Api");
        
        if (!Directory.Exists(basePath))
        {
            basePath = Directory.GetCurrentDirectory();
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
