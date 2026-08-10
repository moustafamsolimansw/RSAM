using Microsoft.EntityFrameworkCore;
using RSAM.Domain.UserAR;

namespace RSAM.Infrastructure.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
    {
        throw new NotImplementedException();
    }
}
