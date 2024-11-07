using Microsoft.Extensions.DependencyInjection.Extensions;
using NetLah.Extensions.ClockProvider;

namespace Microsoft.Extensions.DependencyInjection;

public static class ClockProviderExtensions
{
    public static IServiceCollection AddClockProvider(this IServiceCollection services, bool addUnderlyingProvider = false)
    {

#if NET8_0_OR_GREATER
        if (addUnderlyingProvider)
        {
            services.TryAddSingleton(TimeProvider.System);
        }
#endif

        services.TryAddSingleton<IClockProvider, ClockProvider>();
        return services;
    }

    public static IServiceCollection AddLegacyClockProvider(this IServiceCollection services, bool addUnderlyingProvider = false)
    {

#if NET8_0_OR_GREATER
        services.AddClockProvider(addUnderlyingProvider);
#else
        if (addUnderlyingProvider)
        {
            services.TryAddSingleton<Microsoft.AspNetCore.Authentication.ISystemClock, Microsoft.AspNetCore.Authentication.SystemClock>();
        }
        services.TryAddSingleton<IClockProvider, LegacyClockProvider>();
#endif

        return services;
    }

}
