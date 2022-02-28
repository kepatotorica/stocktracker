using HtmlAgilityPack;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Linq;

namespace Stock.Web.Scraper.Service.Objects
{
    public class YahooStock
    {
        private decimal? CurrentPrice { get; set; }
        public string Ticker { get; set; }
        public DateTime DateAdded { get; set; }
        public decimal? Price { get; set; }
        public decimal? DayPrice { get; set; }
        public decimal? FiveDayPrice { get; set; }
        public decimal? TenDayPrice { get; set; }

        //public decimal? MonthPrice { get; set; }
        //public decimal? ThreeMonthPrice { get; set; }
        //public decimal? YearPrice { get; set; }

        public YahooStock(string ticker = null, DateTime? dateAdded = null)
        {
            Ticker = ticker;
            DateAdded = dateAdded ?? DateTime.UtcNow;//TODOASDF should we subtract a day here?
            //CurrentPrice = GetCurrentPrice();
        }

        public decimal? GetCurrentPrice()
        {
            HtmlDocument doc = new HtmlWeb().Load($"https://finance.yahoo.com/quote/{Ticker}?p={Ticker}");

            return Decimal.Parse(doc.DocumentNode.SelectNodes(ScraperInfo.StockPageIds.Open).First().InnerHtml);
        }

        public void UpdatePrices()
        {
            DateTime now = DateTime.UtcNow;
            if (DateAdded > now.AddMinutes(-10) && DateAdded <= now) // First Time Being Added
            {
                DayPrice = CurrentPrice;
            }
            else if (DateAdded > now.AddHours(-24) && DateAdded <= now) // After one day
            {
                DayPrice = CurrentPrice;
            }
        }

    }
}
