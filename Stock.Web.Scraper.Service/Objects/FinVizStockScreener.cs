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
        public List<YahooStock> Stocks { get; set; }

        public FinVizStockScreener((string title, string screenerUrl) screenerSrapingData)
        {
            Title = screenerSrapingData.title;
            ScreenerUrl = screenerSrapingData.screenerUrl;
            UpdateScreener();
        }

        public FinVizStockScreener UpdateScreener()
        {
            //TODOASDF need to rework all of this to work with finviz
            try
            {
                HtmlDocument doc = new HtmlWeb().Load(ScreenerUrl);
                var tickers = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowNames).Select(rows => rows.InnerHtml);
                var prices = doc.DocumentNode.SelectNodes(ScraperInfo.ScreenerPageIds.RowPrice).Select(rows => Decimal.Parse(rows.InnerHtml));
                Stocks = tickers.Zip(prices).Select(tuple => new YahooStock
                {
                    Ticker = tuple.First,
                    CurrentPrice = tuple.Second
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Screener");

            }
            return this;
        }
    }
}
