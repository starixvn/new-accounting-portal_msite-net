namespace Maintenance.Letstalk.Services;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public interface IHttpClientService
{
    T PostAsync<T, P>(string action, P inputParams);
}

public class HttpClientService(IConfiguration configuration) : IHttpClientService
{
    private static readonly HttpClient Client = CreateHttpClient();

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        return client;
    }

    public T PostAsync<T, P>(string action, P inputParams)
    {
        var apiUrl = configuration.GetValue<string>("Letstalk:LetstalkApiUrl");
        var letstalkApiUrl = $"{apiUrl}/Letstalk/{action}";
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(JsonConvert.SerializeObject(inputParams), Encoding.UTF8, "application/json");

        var responseResult = Client.PostAsync(letstalkApiUrl, content).GetAwaiter().GetResult();

        if (responseResult.IsSuccessStatusCode)
        {
            var data = responseResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data.Result);
        }

        return default;
    }
}