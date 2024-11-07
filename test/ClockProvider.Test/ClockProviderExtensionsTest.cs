using Microsoft.Extensions.DependencyInjection;
#if !NET8_0_OR_GREATER
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;
using SystemClock = Microsoft.AspNetCore.Authentication.SystemClock;
#endif

namespace NetLah.Extensions.ClockProvider.Test;

public class ClockProviderExtensionsTest
{
    private static void AddClockProvider(IServiceCollection services, bool add = false) => services.AddClockProvider(add);
    private static void AddLegacyClockProvider(IServiceCollection services, bool add = false) => services.AddLegacyClockProvider(add);

#if NET8_0_OR_GREATER

    [Fact]
    public async Task ClockProvider_Work_Test80()
    {
        var serviceCollection = new ServiceCollection();
        IServiceCollection services = serviceCollection;
        AddClockProvider(services, true);

        var serviceProvider = services.BuildServiceProvider();

        var clockProvider = serviceProvider.GetService<IClockProvider>();
        Assert.IsType<ClockProvider>(clockProvider);
        Assert.IsAssignableFrom<TimeProvider>(clockProvider.Provider);

        var utc1 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc1);

        await Task.Delay(100);
        var utc2 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc2);
        Assert.NotEqual(utc1, utc2);
        Assert.True(utc1 < utc2);
    }

    [Fact]
    public async Task LegacyClockProvider_Work_Test80()
    {
        var serviceCollection = new ServiceCollection();
        IServiceCollection services = serviceCollection;
        AddLegacyClockProvider(services, true);

        var serviceProvider = services.BuildServiceProvider();

        var clockProvider = serviceProvider.GetService<IClockProvider>();
        Assert.IsType<ClockProvider>(clockProvider);
        Assert.IsAssignableFrom<TimeProvider>(clockProvider.Provider);

        var utc1 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc1);

        await Task.Delay(100);
        var utc2 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc2);
        Assert.NotEqual(utc1, utc2);
        Assert.True(utc1 < utc2);
    }

    [Fact]
    public void DefaultClockProviderTest80()
    {
        var serviceCollection = new ServiceCollection();
        IServiceCollection services = serviceCollection;

        Assert.Empty(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddClockProvider(services);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddClockProvider(services);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddClockProvider(services, true);
        Assert.Equal(2, serviceCollection.Count);
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddClockProvider(services, true);
        Assert.Equal(2, serviceCollection.Count);
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));
    }

    [Fact]
    public void LegacyClockProviderTest80()
    {
        var serviceCollection = new ServiceCollection();
        IServiceCollection services = serviceCollection;

        Assert.Empty(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddLegacyClockProvider(services);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddLegacyClockProvider(services);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddLegacyClockProvider(services, true);
        Assert.Equal(2, serviceCollection.Count);
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddLegacyClockProvider(services, true);
        Assert.Equal(2, serviceCollection.Count);
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(TimeProvider)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));
    }

#else

    [Fact]
    public async Task ClockProvider_Work_Test60()
    {
        var serviceCollection = new ServiceCollection();
        IServiceCollection services = serviceCollection;
        AddClockProvider(services);

        var serviceProvider = services.BuildServiceProvider();

        var clockProvider = serviceProvider.GetService<IClockProvider>();
        Assert.IsType<ClockProvider>(clockProvider);
        Assert.IsType<ClockProvider>(clockProvider.Provider);
        Assert.Null(serviceProvider.GetService<ISystemClock>());

        var utc1 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc1);

        await Task.Delay(100);
        var utc2 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc2);
        Assert.NotEqual(utc1, utc2);
        Assert.True(utc1 < utc2);
    }

    [Fact]
    public async Task LegacyClockProvider_Work_Test60()
    {
        var serviceCollection = new ServiceCollection();
        IServiceCollection services = serviceCollection;
        AddLegacyClockProvider(services, true);

        var serviceProvider = services.BuildServiceProvider();

        var clockProvider = serviceProvider.GetService<IClockProvider>();
        Assert.IsType<LegacyClockProvider>(clockProvider);
        Assert.IsType<SystemClock>(serviceProvider.GetService<ISystemClock>());
        Assert.IsType<SystemClock>(clockProvider.Provider);

        var max = 20;
        var utcNowPrecisionSecondsStart = new DateTime(DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond, DateTimeKind.Utc);
        while (max > 0 && (new DateTime(DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond, DateTimeKind.Utc) == utcNowPrecisionSecondsStart))
        {
            max--;
            await Task.Delay(100);
        }

        var utc1 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc1);

        await Task.Delay(100);
        var utc2 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc2);
        Assert.Equal(utc1, utc2);
        Assert.False(utc1 < utc2);

        await Task.Delay(1100);
        var utc3 = clockProvider.GetUtcNow();
        Assert.NotEqual(DateTimeOffset.MinValue, utc3);
        Assert.NotEqual(utc1, utc3);
        Assert.True(utc1 < utc3);
    }

    [Fact]
    public void DefaultClockProviderTest60()
    {
        var serviceCollection = new ServiceCollection();
        IServiceCollection services = serviceCollection;

        Assert.Empty(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddClockProvider(services);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddClockProvider(services);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddClockProvider(services, true);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddClockProvider(services, true);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));
    }

    [Fact]
    public void LegacyClockProviderTest60()
    {
        var serviceCollection = new ServiceCollection();
        IServiceCollection services = serviceCollection;

        Assert.Empty(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddLegacyClockProvider(services);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddLegacyClockProvider(services);
        Assert.Single(serviceCollection);
        Assert.Equal(0, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddLegacyClockProvider(services, true);
        Assert.Equal(2, serviceCollection.Count);
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));

        AddLegacyClockProvider(services, true);
        Assert.Equal(2, serviceCollection.Count);
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(ISystemClock)));
        Assert.Equal(1, services.Count(x => x.ServiceType == typeof(IClockProvider)));
    }

#endif

}
