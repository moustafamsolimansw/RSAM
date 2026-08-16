using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSAM.Domain.UserAR.Entities;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Infrastructure.Config;

public class UserOTPConfig : IEntityTypeConfiguration<UserOTP>
{
    public void Configure(EntityTypeBuilder<UserOTP> builder)
    {
        builder.ToTable("UserOTPs");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasConversion(
            id => id.Value,
            value => UserOTPId.Create(value)
        );
        builder.Property(u => u.UserId).IsRequired().HasConversion(
            userId => userId.Value,
            value => UserId.Create(value)
        );
        builder.Property(u => u.Purpose).IsRequired().HasConversion<string>();
        builder.Property(u => u.Channel).IsRequired().HasConversion<string>();
        builder.Property(u => u.HashedOTP).IsRequired();
        builder.Property(u => u.ExpireAt).IsRequired();
    }
}
