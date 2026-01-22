namespace StockApp.ServiceContracts;

public interface IFinnHubService
{
    Task<Dictionary<string, object>?> GetStorckPriceQuote(string stocksymbol);
}