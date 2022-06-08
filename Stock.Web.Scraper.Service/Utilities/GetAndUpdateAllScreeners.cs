using HtmlAgilityPack;
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
        stocks.ForEach(stock => stock.UpdatePrices(GetCurrentPrice(stock)));
      }
      catch { }
      return stocks;
    }

    private decimal GetCurrentPrice(StockRow stock)
    {
      try
      {
        if (!_memoryCache.TryGetValue($"{stock.Ticker}{DateTime.Now:MM/dd/yy}", out decimal todaysPrice))
        {
          var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));

          //TODOASDF Make an API call to IDEX here, if it doesn't work, then and only then should you grab it from finviz
          HtmlDocument doc = new HtmlWeb().Load($"https://finviz.com/quote.ashx?t={stock.Ticker}");
          todaysPrice = Decimal.Round(Decimal.Parse(doc.DocumentNode.SelectNodes(ScraperXpaths.StockPageIds.CurrentValue).First().InnerHtml), 2);

          if (todaysPrice == null)
            todaysPrice = 0;

          _memoryCache.Set($"{stock}{DateTime.Now:MM/dd/yy}", todaysPrice, cacheEntryOptions);
        }

        return todaysPrice;
      }
      catch
      {
        return 0m;
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