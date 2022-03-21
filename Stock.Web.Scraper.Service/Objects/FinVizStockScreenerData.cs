using HtmlAgilityPack;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Web.Scraper.Service.Objects
{
  public class FinVizStockScreenerData
  {
    public string Title { get; set; }
    public string ScreenerUrl { get; set; }
    public List<ScreenerRowData> Stocks { get; set; } = new List<ScreenerRowData>();

    public FinVizStockScreenerData((string title, string screenerUrl) screenerSrapingData)
    {
      Title = screenerSrapingData.title;
      ScreenerUrl = screenerSrapingData.screenerUrl;
    }

    public FinVizStockScreenerData UpdateScreener()
    {
      try
      {
        HtmlDocument doc = new HtmlWeb().Load(ScreenerUrl);
        var tickers = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowNames).Select(rows => rows.InnerHtml);
        var TODOASDFDELETEME = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowPrice).ToList();

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
            CurrentPrice = price
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
