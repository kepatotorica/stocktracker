using HtmlAgilityPack;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Web.Scraper.Service.Objects
{
  public class FinVizStockScreener
  {
    public string Title { get; set; }
    public string ScreenerUrl { get; set; }
    public List<ScreenerRowData> Stocks { get; set; } = new List<ScreenerRowData>();

    public FinVizStockScreener((string title, string screenerUrl) screenerSrapingData)
    {
      Title = screenerSrapingData.title;
      ScreenerUrl = screenerSrapingData.screenerUrl;
    }

    public FinVizStockScreener ScrapeCurrentScreenerData()
    {
      try
      {
        HtmlDocument doc = new HtmlWeb().Load(ScreenerUrl);
        var tickers = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowNames).Select(rows => rows.InnerHtml);

        Stocks = tickers.Select(ticker =>
        {
          var priceText = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowPriceWithTicker(ticker))
                      .Where((x, i) => i == 8)
                      .First()
                      .FirstChild
                      .InnerHtml;

          var price = 0m;
          decimal.TryParse(priceText, out price);

          return new ScreenerRowData
          {
            Ticker = ticker,
            CurrentPrice = price,
            PriceWhenAdded = price,
            DateAdded = DateTime.Now.ToString("MM/dd/yy")
          };
        }).ToList();
      }
      catch { }
      return this;
    }

    public void AddRows(IEnumerable<ScreenerRowData> rowsToAdd)
    {
      rowsToAdd = rowsToAdd ?? new List<ScreenerRowData>();
      var cleanedStocks = Stocks.Where(scrappedRows => !rowsToAdd.Any(existingRows => existingRows.Equals(scrappedRows)));
      Stocks = rowsToAdd.Concat(cleanedStocks).ToList();
    }

  }
}
