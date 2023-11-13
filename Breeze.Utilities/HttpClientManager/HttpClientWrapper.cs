using Breeze.Utilities.HttpClientManager;
using Newtonsoft.Json;
using System.Text;

namespace Breeze.Utilities;
public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResponse?> GetAsync<TResponse, TRequest>(string url, TRequest request)
    {
        string queryString = request is null ? "" : $"?{Uri.EscapeDataString(request.ToString() ?? string.Empty)}";
        HttpResponseMessage response = await _httpClient.GetAsync($"{url}{queryString}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        string responseBody = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(responseBody);
    }

    public async Task<TResponse?> PostAsync<TResponse, TRequest>(string url, TRequest request, string contentType)
    {
        StringContent stringContent = new(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

        HttpResponseMessage response = await _httpClient.PostAsync(url, stringContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        string responseBody = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TResponse>(responseBody);
    }
}