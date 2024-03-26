namespace Maintenance.BotService.Services
{
    using System.Text;
    using System.Threading.Tasks;
    using Maintenance.BotService.Models.Message;
    using Maintenance.BotService.Services.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class BotConnector(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IBotConnector
    {
        private readonly string BotServiceDomainUrl = configuration.GetValue<string>("BotService:ApiUrl");

        public async Task RetryMessageAsync(MessageResend message)
        {
            await PostAsync<object, MessageResend>("api/retry", message);
        }

        public async Task<T> PostAsync<T, P>(string action, P data)
        {
            string url = $"{BotServiceDomainUrl}/{action}";
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var client = httpClientFactory.CreateClient();
            var responseResult = await client.PostAsync(url, content);
            responseResult.EnsureSuccessStatusCode();
            if (responseResult.IsSuccessStatusCode)
            {
                var result = await responseResult.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }

            return default;
        }
    }
}