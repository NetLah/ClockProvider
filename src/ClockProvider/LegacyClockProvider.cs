namespace NetLah.Extensions.ClockProvider;

#if !NET8_0_OR_GREATER

public class LegacyClockProvider(Microsoft.AspNetCore.Authentication.ISystemClock systemClock) : IClockProvider
{
    public DateTimeOffset GetUtcNow() => Provider.UtcNow;

    object IClockProvider.Provider => Provider;

    public Microsoft.AspNetCore.Authentication.ISystemClock Provider { get; } = systemClock;
}

#endif
