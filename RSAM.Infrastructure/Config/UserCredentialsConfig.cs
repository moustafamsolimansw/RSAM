using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.Entities;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Infrastructure.Config;

public class UserCredentialsConfig : IEntityTypeConfiguration<RSAM.Domain.UserAR.Entities.UserCredentials>
{
    public void Configure(EntityTypeBuilder<UserCredentials> builder)
    {
        builder.ToTable("UserCredentials");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasConversion(x => x.Value, x => UserCredentialsId.Create(x));
        builder.HasOne<User>()
            .WithMany(u => u.UserCredentials)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
