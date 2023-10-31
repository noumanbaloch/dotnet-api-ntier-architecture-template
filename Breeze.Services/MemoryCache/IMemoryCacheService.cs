
namespace Breeze.Services.MemoryCache;
public interface IMemoryCacheService
{
    T? Get<T>(string cachedKey);
    void Set<T>(string cachedKey, T value);
    void Remove(string cachedKey);
    void Remove(IEnumerable<string> cachedKeys);
    IEnumerable<string> GetKeysContain(string pattern);
}