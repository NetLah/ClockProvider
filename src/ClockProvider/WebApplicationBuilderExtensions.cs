using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddClockProvider(this WebApplicationBuilder webApplicationBuilder, bool addUnderlyingProvider = false)
    {
        webApplicationBuilder.Services.AddClockProvider(addUnderlyingProvider);
        return webApplicationBuilder;
    }

    public static WebApplicationBuilder AddLegacyClockProvider(this WebApplicationBuilder webApplicationBuilder, bool addUnderlyingProvider = false)
    {
        webApplicationBuilder.Services.AddLegacyClockProvider(addUnderlyingProvider);
        return webApplicationBuilder;
    }
}
