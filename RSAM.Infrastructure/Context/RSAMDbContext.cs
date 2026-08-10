using Microsoft.EntityFrameworkCore;
using RSAM.Infrastructure.Config;

namespace RSAM.Infrastructure.Context;

public class RSAMDbContext : DbContext
{
    public const string DbTablePrefix = "";
    public const string DbSchema = "public";

    public const string DatabaseName = "RSAM";

    #region DbSets
    
    #endregion

    public RSAMDbContext(DbContextOptions<RSAMDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        if (!string.IsNullOrWhiteSpace(DbSchema))
        {
            modelBuilder.HasDefaultSchema(DbSchema);
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RSAMDbContext).Assembly);
    }
}
