using Microsoft.EntityFrameworkCore;
using RSAM.Domain.ValueObjects;
using RSAM.Infrastructure.Config;

namespace RSAM.Infrastructure.Context;

public class RSAMDbContext : DbContext
{
    public const string DbTablePrefix = "";
    public const string DbSchema = "public";

    public const string DatabaseName = "RSAM";

    #region DbSets
    public DbSet<RSAM.Domain.UserAR.User> Users { get; set; }
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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
        modelBuilder.Ignore<EmailAddress>();
        modelBuilder.Ignore<Address>();
        modelBuilder.Ignore<PersonInfo>();
    }
}
