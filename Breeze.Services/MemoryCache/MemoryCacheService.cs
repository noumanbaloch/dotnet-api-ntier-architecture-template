using Breeze.Utilities;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace Breeze.Services.MemoryCache;

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IAppCache _cache;
    private List<string> CachedKeys = [];

    public MemoryCacheService(IAppCache cache)
    {
        _cache = cache;
    }

    public T? Get<T>(string cachedKey)
    {
        if (_cache.TryGetValue(cachedKey, out T value))
        {
            return value;
        }

        return default;
    }

    public void Set<T>(string cachedKey, T value)
    {
        _cache.Add(cachedKey, value, GetDefaultCacheEntryOptions());
        CachedKeys.Add(cachedKey);
    }

    public void Remove(string cachedKey)
    {
        _cache.Remove(cachedKey);
        CachedKeys.Remove(cachedKey);
    }

    public void Remove(IEnumerable<string> cachedKeys)
    {
        var cachedKeysToRemove = cachedKeys.ToList();
        foreach (var cachedKey in cachedKeysToRemove)
        {
            _cache.Remove(cachedKey);
            CachedKeys.Remove(cachedKey);
        }
    }

    public IEnumerable<string> GetKeysContain(string pattern)
    {
        return CachedKeys.Where(key => key.Contains(pattern));
    }

    #region Private Methods
    private static MemoryCacheEntryOptions GetDefaultCacheEntryOptions()
    {
        return new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddDays(1),
            SlidingExpiration = TimeSpan.FromMinutes(60)
        };
    }
    #endregion
}