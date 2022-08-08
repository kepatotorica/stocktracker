using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stock.Web.Scraper.Service.Utilities
{
  public class GetAndUpdateAllScreeners
  {
    public string screenerCsvPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\Screeners\\";
    public string summaryCsvPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\";
    public string screenerJsonPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\Screeners.json";
    public List<ScreenerTableScraper> screeners = new List<ScreenerTableScraper>();
    private readonly IMemoryCache _memoryCache;

    public GetAndUpdateAllScreeners(IMemoryCache memoryCache) => _memoryCache = memoryCache;

    public void ScrapeAndUpdate()
    {
      var screenerInfo = JArray.Parse(File.ReadAllText(screenerJsonPath)).Select(jsonItem => jsonItem.ToObject<ScreenerInfo>()).ToList();

      screenerInfo.ForEach(UpdateCsv);
      var summaries = screeners.Select(screener => new SummaryRow(screener)).OrderByDescending(summary => summary.DayPercent);
      UpdateScreenerSummaryCSV(summaries);
    }

    private void UpdateCsv(ScreenerInfo s)
    {
      Console.WriteLine("Updating Screener " + s.Title);
      try
      {
        var screener = new ScreenerTableScraper(s, _memoryCache).ScrapeCurrentScreenerData();
        screener.AddRows(GetExistingRows(s));
        screeners.Add(screener);
        UpdateScreenerCsv(screener);
      }
      catch { }
    }

    private IEnumerable<StockRow> GetExistingRows(ScreenerInfo s)
    {
      var stocks = new List<StockRow>();
      try
      {
        stocks = $"{screenerCsvPath}{s.Title}".ReadFromCsv<StockRow>().ToList();
        UpdateCurrentPrices(stocks);
        stocks.ForEach(stock => stock.UpdatePrices(stock.CurrentPrice));
      }
      catch { }
      return stocks;
    }

    private void UpdateCurrentPrices(IEnumerable<StockRow> stocks)
    {
      List<string> tickersThatNeedUpdating = new List<string>();
      foreach (var stock in stocks)
      {
        if (!_memoryCache.TryGetValue($"{stock.Ticker}{DateTime.Now:MM/dd/yy}", out decimal todaysPrice))
        {
          stock.CurrentPrice = todaysPrice;

          tickersThatNeedUpdating.Add(stock.Ticker);
        }
      }

      var stocksFromApi = IexAPI.GetTickers(tickersThatNeedUpdating);

      var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));
      foreach (var stock in stocksFromApi)
      {
        _memoryCache.Set($"{stock.symbol}{DateTime.Now:MM/dd/yy}", stock.lastSalePrice, cacheEntryOptions);
      }

      foreach (var stock in stocks)
      {
        _memoryCache.TryGetValue($"{stock.Ticker}{DateTime.Now:MM/dd/yy}", out decimal todaysPrice);
        stock.CurrentPrice = todaysPrice;
      }
    }

    private void UpdateScreenerCsv(ScreenerTableScraper obj)
    {
      obj.Stocks.WriteToCsv($"{screenerCsvPath}{obj.Title}");
    }

    private void UpdateScreenerSummaryCSV(IEnumerable<SummaryRow> screenerSummaries)
    {
      screenerSummaries.WriteToCsv($"{summaryCsvPath}Summary");
    }
  }
}