
namespace Breeze.Services.MemoryCache;
public interface IMemoryCacheService
{
    T Get<T>(string key);
    void Set<T>(string key, T value);
    void Remove(string key);
}