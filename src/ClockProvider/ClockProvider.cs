namespace NetLah.Extensions.ClockProvider;

#if NET8_0_OR_GREATER

public class ClockProvider(TimeProvider timeProvider) : IClockProvider
{
    public DateTimeOffset GetUtcNow() => Provider.GetUtcNow();

    object IClockProvider.Provider => Provider;

    public TimeProvider Provider { get; } = timeProvider;
}

#else

public class ClockProvider : IClockProvider
{
    public DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;

    public object Provider => this;
}

#endif
