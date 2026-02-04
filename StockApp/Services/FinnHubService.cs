using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StockApp.ServiceContracts;

namespace StockApp.Services;

public class FinnHubService : IFinnHubService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public FinnHubService(IHttpClientFactory httpClientFactory ,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<Dictionary<string,object>?> GetStorckPriceQuote(string? stockSymbol)
    {
        using (HttpClient client = _httpClientFactory.CreateClient())
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnHubToken"]}"),
                Method = HttpMethod.Get 
            };
            
            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
            
            Stream stream = responseMessage.Content.ReadAsStream();
            
            StreamReader streamReader = new StreamReader(stream);
            
            string responseBody = streamReader.ReadToEnd();
            
            Dictionary<string , object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string , object>? >(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("no response from finnhub service");
            
            if(responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(responseDictionary["error"].ToString());
            return responseDictionary;
        }
    }

}