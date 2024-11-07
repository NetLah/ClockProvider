namespace NetLah.Extensions.ClockProvider;

public interface IClockProvider
{
    public DateTimeOffset GetUtcNow();

    public object Provider { get; }
}
