using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Web.Scraper.Service.Utilities
{
  public class ScreenerTableScraper
  {
    public string Title { get; set; }
    public string Url { get; set; }

    private readonly IMemoryCache _memoryCache;

    public List<StockRow> Stocks { get; set; } = new List<StockRow>();

    public ScreenerTableScraper(ScreenerInfo screenerSrapingData, IMemoryCache memoryCache)
    {
      Title = screenerSrapingData.Title;
      Url = screenerSrapingData.Url;
      _memoryCache = memoryCache;
    }

    public ScreenerTableScraper ScrapeCurrentScreenerData()
    {
      try
      {
        HtmlDocument doc = new HtmlWeb().Load(Url);
        var tickers = doc.DocumentNode.SelectNodes(ScraperXpaths.ScreenerPageIds.RowNames)?.Select(rows => rows.InnerHtml) ?? new List<string>();

        Stocks = tickers.Select(ticker =>
        {
          var priceText = doc.DocumentNode.SelectNodes(ScraperXpaths.ScreenerPageIds.RowPriceWithTicker(ticker))
                      .Where((x, i) => i == 8)
                      .First()
                      .FirstChild
                      .InnerHtml;

          var price = 0m;
          decimal.TryParse(priceText, out price);

          return new StockRow()
          {
            Ticker = ticker,
            CurrentPrice = price,
            PriceWhenAdded = price,
            DateAdded = DateTime.Now.ToString("MM/dd/yy")
          };
        }).Where(row => row.PriceWhenAdded != 0).ToList();
      }
      catch { }
      return this;
    }

    public void AddRows(IEnumerable<StockRow> rowsToAdd)
    {
      rowsToAdd = rowsToAdd ?? new List<StockRow>();
      var cleanedStocks = Stocks.Where(scrappedRows => !rowsToAdd.Any(existingRows => existingRows.Equals(scrappedRows)));
      Stocks = rowsToAdd.Concat(cleanedStocks).ToList();
    }
  }
}
