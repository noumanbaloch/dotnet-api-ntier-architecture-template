using System.Net.Http.Headers;

namespace Breeze.Utilities.HttpClientManager;

public interface IHttpClientWrapper
{
    Task<TResponse?> GetAsync<TResponse, TRequest>(string uri, AuthenticationHeaderValue? authenticationHeader = default, IEnumerable<KeyValuePair<string, string>>? customHeaders = default, TRequest? request = default);
    Task<TResponse?> PostAsync<TResponse, TRequest>(string uri, AuthenticationHeaderValue? authenticationHeader = default, IEnumerable<KeyValuePair<string, string>>? customHeaders = default, TRequest? request = default);
}