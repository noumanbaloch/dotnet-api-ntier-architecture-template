using System.Collections.Concurrent;

namespace Breeze.Services.Cache;
public class CacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, object> _cache = new();

    public T GetData<T>(string key)
    {
        if (_cache.TryGetValue(key, out var value))
        {
            return (T)value;
        }

        return default!;
    }

    public bool SetData<T>(string key, T value)
    {
        _cache.AddOrUpdate(key, value!, (k, v) => value!);
        return true;
    }

    public bool RemoveData(string key)
    {
        return _cache.TryRemove(key, out _);
    }

    public bool Exists(string key)
    {
        return _cache.ContainsKey(key);
    }
}
