using RSAM.Application.Time;

namespace RSAM.Infrastructure.Time;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
