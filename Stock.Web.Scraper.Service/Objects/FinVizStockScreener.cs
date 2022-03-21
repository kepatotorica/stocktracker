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

        public FinVizStockScreener UpdateScreener()
        {
            try
            {
                HtmlDocument doc = new HtmlWeb().Load(ScreenerUrl);
                var tickers = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowNames).Select(rows => rows.InnerHtml);
                var prices = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowPrice).Where((x, i) => (i - 1) % 2 == 0).Select(rows => Decimal.Parse(rows.InnerHtml));
                Stocks = tickers.Zip(prices).Select(tuple => new ScreenerRowData
                {
                    Ticker = tuple.First,
                    CurrentPrice = tuple.Second
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("INVALID SCREENER");
            }
            return this;
        }

        public void AddRows(IEnumerable<ScreenerRowData> rowsToAdd)
        {
            rowsToAdd = rowsToAdd ?? new List<ScreenerRowData>();
            Stocks = rowsToAdd.Concat(Stocks).ToList();
        }
    }
}
