using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Maintenance.Mattermost.Services;

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
        var apiUrl = configuration.GetValue<string>("Mattermost:MattermostApiUrl");
        var mattermostApiUrl = $"{apiUrl}/Mattermost/{action}";
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(JsonConvert.SerializeObject(inputParams), Encoding.UTF8, "application/json");

        var responseResult = Client.PostAsync(mattermostApiUrl, content).GetAwaiter().GetResult();

        if (responseResult.IsSuccessStatusCode)
        {
            var data = responseResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data.Result);
        }

        return default;
    }
}