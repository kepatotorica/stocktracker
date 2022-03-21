using HtmlAgilityPack;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Linq;

namespace Stock.Web.Scraper.Service.Objects
{
    public class ScreenerRowData
    {
        public string Ticker { get; set; }
        public DateTime? DateAdded { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? PriceWhenAdded { get; set; }
        public decimal? DayPrice { get; set; }
        public decimal? FiveDayPrice { get; set; }
        public decimal? TenDayPrice { get; set; }

        //public decimal? MonthPrice { get; set; }
        //public decimal? ThreeMonthPrice { get; set; }
        //public decimal? YearPrice { get; set; }

        public ScreenerRowData(string ticker = null, string dateAdded = null, decimal? currentPrice = null)
        {
            try
            {
                DateAdded = DateTime.Parse(dateAdded);
            }
            catch
            {
                DateAdded = DateTime.UtcNow;
            }

            this.Ticker = ticker;
            this.CurrentPrice = currentPrice;
        }

        public decimal? GetOpenPrice()
        {
            HtmlDocument doc = new HtmlWeb().Load($"https://finance.yahoo.com/quote/{Ticker}?p={Ticker}");

            return Decimal.Parse(doc.DocumentNode.SelectNodes(ScraperInfo.StockPageIds.Open).First().InnerHtml);
        }

        public void UpdatePrices()
        {
            //TODOASDF Find a way to cache all of the prices for any stock so we don't have to keep scraping data if the same stock pops up
            try
            {
                DateTime now = DateTime.UtcNow;
                if (DateAdded > now.AddMinutes(-10) && DateAdded <= now) // First Time Being Added
                {
                    PriceWhenAdded = CurrentPrice;
                }
                else if (DateAdded > now.AddHours(-24) && DateAdded <= now) // After one day
                {
                    DayPrice = CurrentPrice;
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
