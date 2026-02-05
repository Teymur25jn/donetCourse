using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.OptionsModels;
using StockApp.ServiceContracts;
using StockApp.Services;
using StockApp.ViewModels;

namespace StockApp.Controllers;

public class HomeController : Controller
{
    private readonly FinnHubService _finnHubService;
    private readonly IOptions<TradingOptionsForFinnHub> _tradingOptions;

    public HomeController(FinnHubService finnHubService ,
        IOptions<TradingOptionsForFinnHub> tradingOptions)
    {
        _finnHubService = finnHubService;
        _tradingOptions = tradingOptions;
    }
    
    // GET
    [Route("/")]
    [Route("/home")]
    public async Task<IActionResult> Index()
    {
        if (string.IsNullOrWhiteSpace(_tradingOptions.Value.DefaultStockSymbol))
        {
            _tradingOptions.Value.DefaultStockSymbol = "MSFT";
        }
        
        Dictionary<string,object>? responseDictionary = await _finnHubService.GetStorckPriceQuote(_tradingOptions.Value.DefaultStockSymbol);

        StockViewModel stockViewModel = new StockViewModel()
        {
            StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
            CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),
            HighestPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
            LowestPrice = Convert.ToDouble(responseDictionary["l"].ToString()),
            OpenPrice = Convert.ToDouble(responseDictionary["o"].ToString()),
        };
        
        return View(stockViewModel);
    }
}