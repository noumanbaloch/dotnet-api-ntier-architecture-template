using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Breeze.Utilities.HttpClientManager;
public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResponse?> GetAsync<TResponse, TRequest>(string uri, AuthenticationHeaderValue? authenticationHeader = default, IEnumerable<KeyValuePair<string, string>>? customHeaders = default, TRequest? request = default)
    {
        if (authenticationHeader != null)
        {
            ConfigureAuthorizationHeader(authenticationHeader);
        }

        if (customHeaders != null)
        {
            ConfigureCustomRequestHeaders(customHeaders);
        }

        string queryString = request is null ? "" : $"?{Uri.EscapeDataString(request.ToString() ?? string.Empty)}";
        var response = await _httpClient.GetAsync($"{uri}{queryString}");

        ValidateResponse(response);

        string responseBody = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(responseBody);
    }

    public async Task<TResponse?> PostAsync<TResponse, TRequest>(string uri, AuthenticationHeaderValue? authenticationHeader = default, IEnumerable<KeyValuePair<string, string>>? customHeaders = default, TRequest? request = default)
    {
        StringContent stringContent = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        if (authenticationHeader != null)
        {
            ConfigureAuthorizationHeader(authenticationHeader);
        }

        if (customHeaders != null)
        {
            ConfigureCustomRequestHeaders(customHeaders);
        }

        var response = await _httpClient.PostAsync(uri, stringContent);

        ValidateResponse(response);

        string responseBody = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(responseBody);
    }

    private void ConfigureAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }

    private void ConfigureCustomRequestHeaders(IEnumerable<KeyValuePair<string, string>> customHeaders)
    {
        foreach(var header in customHeaders)
        {
            ReplaceOrAddHttpHeader(header);
        }
    }

    private void ReplaceOrAddHttpHeader(KeyValuePair<string, string> header)
    {
        if(_httpClient.DefaultRequestHeaders.Contains(header.Key))
        {
            _httpClient.DefaultRequestHeaders.Remove(header.Key);
        }

        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
    }

    private static void ValidateResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }
}