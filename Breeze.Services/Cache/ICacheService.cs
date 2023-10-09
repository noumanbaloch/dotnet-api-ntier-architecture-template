
namespace Breeze.Services.Cache;
public interface ICacheService
{
    T GetData<T>(string key);
    bool SetData<T>(string key, T value);
    bool RemoveData(string key);
    bool Exists(string key);
}