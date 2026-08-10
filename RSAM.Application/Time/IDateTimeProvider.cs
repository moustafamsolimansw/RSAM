namespace RSAM.Application.Time;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
