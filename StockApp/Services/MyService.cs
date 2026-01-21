using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StockApp.Services;

public class MyService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MyService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task TestMethod()
    {
        using (HttpClient client = _httpClientFactory.CreateClient())
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://finnhub.io/api/v1/quote?symbol=MSFT&token=d5o8t09r01qma2b88e2gd5o8t09r01qma2b88e30"),
                Method = HttpMethod.Get 
            };
            
            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
            
            Stream stream = responseMessage.Content.ReadAsStream();
            
            StreamReader streamReader = new StreamReader(stream);
            
            string responseBody = streamReader.ReadToEnd();
            
            Dictionary<string , object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string , object>? >(responseBody);
            
            
        }
    }

}