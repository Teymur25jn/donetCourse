using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockApp.ServiceContracts;

public interface IFinnHubService
{
    Task<Dictionary<string, object>?> GetStorckPriceQuote(string stocksymbol);
}