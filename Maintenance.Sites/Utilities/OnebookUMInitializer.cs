namespace Maintenance.Sites.Utilities;

using Microsoft.Extensions.Caching.Memory;
using ONEbook.UM.AspNetCore.Filters;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using ONEbook.UM.AspNetCore;

public static class OnebookUMInitializer
{
    public static IServiceCollection AddOneBookUm(this IServiceCollection services, IConfiguration configuration)
    {
        var umConfig = new UMClientConfiguration();
        umConfig.Initialize(configuration);

        services.AddSingleton(umConfig);
        services.AddSingleton<CheckUMFilterAttribute>();
        services.AddSingleton<IUMCacheService, MemoryUMCacheService>();
        services.AddSingleton<IUMRestClient, UMRestClient>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
        services.AddSingleton<IMaintenanceService, MaintenanceService>();

        return services;
    }
}

public class MemoryUMCacheService(IMemoryCache cache) : IUMCacheService
{
    public T Get<T>(string key) => cache.Get<T>(key)
    ;

    public void Remove(string key)
    {
        cache.Remove(key);
    }

    public void Set<T>(string key, T value, TimeSpan absoluteExpiration)
        => cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(absoluteExpiration));
}
