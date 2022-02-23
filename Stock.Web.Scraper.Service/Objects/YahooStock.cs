using HtmlAgilityPack;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Linq;

namespace Stock.Web.Scraper.Service.Objects
{
    public class YahooStock
    {
        public string Ticker { get; set; }
        public DateTime DateAdded { get; set; }
        public decimal? Price { get; set; }

        public YahooStock(string ticker = null, DateTime? dateAdded = null)
        {
            Ticker = ticker;
            DateAdded = dateAdded ?? DateTime.UtcNow;//TODOASDF should we subtract a day here?
        }

        public void SetPriceToOpen()
        {
            HtmlDocument doc = new HtmlWeb().Load($"https://finance.yahoo.com/quote/{Ticker}?p={Ticker}");

            Price = Decimal.Parse(doc.DocumentNode.SelectNodes(ScraperInfo.StockPageIds.Open).First().InnerHtml);
        }
    }
}
