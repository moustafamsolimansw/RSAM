using Microsoft.EntityFrameworkCore;
using RSAM.Domain.UserAR;
using RSAM.Domain.UserAR.ValueObjects;

namespace RSAM.Infrastructure.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
    {
        builder.ToTable($"Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => UserId.Create(x));
        builder.Property(x => x.Username).HasMaxLength(50).IsRequired();
        
        builder.ComplexProperty(x => x.EmailAddress, email =>
        {
            email.Property(e => e.Value).HasColumnName("EmailAddress").HasMaxLength(100).IsRequired();
        });
        builder.ComplexProperty(x => x.PersonInfo, person =>
        {
            person.Property(p => p.FirstNameInEnglish).HasColumnName("FirstNameInEnglish").HasMaxLength(50).IsRequired();
            person.Property(p => p.MiddleNameInEnglish).HasColumnName("MiddleNameInEnglish").HasMaxLength(50).IsRequired(false);
            person.Property(p => p.LastNameInEnglish).HasColumnName("LastNameInEnglish").HasMaxLength(50).IsRequired();
            person.Property(p => p.FirstNameInArabic).HasColumnName("FirstNameInArabic").HasMaxLength(50).IsRequired();
            person.Property(p => p.MiddleNameInArabic).HasColumnName("MiddleNameInArabic").HasMaxLength(50).IsRequired(false);
            person.Property(p => p.LastNameInArabic).HasColumnName("LastNameInArabic").HasMaxLength(50).IsRequired();
            person.Property(p => p.Gender).HasColumnName("Gender").HasConversion<string>().HasMaxLength(20).IsRequired();
            person.Property(p => p.DateOfBirth).HasColumnName("DateOfBirth").IsRequired();
        });
        
    }
}
