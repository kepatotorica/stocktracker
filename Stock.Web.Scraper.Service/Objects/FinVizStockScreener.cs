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
        public List<ScreenerRow> Stocks { get; set; } = new List<ScreenerRow>();

        public FinVizStockScreener((string title, string screenerUrl) screenerSrapingData)
        {
            Title = screenerSrapingData.title;
            ScreenerUrl = screenerSrapingData.screenerUrl;
        }

        public FinVizStockScreener UpdateScreener()
        {
            try
            {
                HtmlDocument doc = new HtmlWeb().Load(ScreenerUrl);
                var tickers = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowNames).Select(rows => rows.InnerHtml);
                var TODOASDFDELETEME = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowPrice).ToList();
                var prices = new List<decimal>();

                Stocks = tickers.Select(ticker =>
                {
                    var priceText = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowPriceWithTicker(ticker))
                        .Where((x, i) => i == 8)
                        .First()
                        .FirstChild
                        .InnerHtml;

                    var price = 0m;
                    decimal.TryParse(priceText, out price);

                    return new ScreenerRow
                    {
                        Ticker = ticker,
                        CurrentPrice = price
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("INVALID SCREENER");
            }
            return this;
        }

        public void AddRows(IEnumerable<ScreenerRow> rowsToAdd)
        {
            rowsToAdd = rowsToAdd ?? new List<ScreenerRow>();
            Stocks = rowsToAdd.Concat(Stocks).ToList();
        }
    }
}
