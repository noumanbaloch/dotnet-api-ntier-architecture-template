using Breeze.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace Breeze.Services.MemoryCache;
public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T Get<T>(string key)
    {
        if (_cache.TryGetValue(key, out var value) && value is T)
        {
            return (T)value;
        }

        return default!;
    }

    public void Set<T>(string key, T value)
    {
        _cache.Set(key, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = Helper.GetCurrentDate().AddDays(1)
        });
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
