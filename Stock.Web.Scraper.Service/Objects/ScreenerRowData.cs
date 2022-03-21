using HtmlAgilityPack;
using Stock.Web.Scraper.Service.Utilities;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Linq;

namespace Stock.Web.Scraper.Service.Objects
{
  public class ScreenerRowData
  {
    public string Ticker { get; set; }
    public decimal? CurrentPrice { get; set; }
    public decimal? PriceWhenAdded { get; set; }
    public DateTime? DateAdded { get; set; }
    public decimal? DayPrice { get; set; }
    public decimal? FiveDayPrice { get; set; }
    public decimal? TenDayPrice { get; set; }
    public decimal? MonthPrice { get; set; }
    public decimal? ThreeMonthPrice { get; set; }
    public decimal? YearPrice { get; set; }

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

      if (PriceWhenAdded == null)
      {
        this.PriceWhenAdded = currentPrice;
      }
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
        if (DateAdded > now.AddMinutes(-10) && DateAdded <= now.AddMinutes(10)) // First Time Being Added
        {
          PriceWhenAdded = CurrentPrice;
        }
        else if (DateAdded.DaysAgo(1))
        {
          DayPrice = CurrentPrice;
        }
        else if (DateAdded.DaysAgo(5))
        {
          FiveDayPrice = CurrentPrice;
        }
        else if (DateAdded.DaysAgo(10))
        {
          TenDayPrice = CurrentPrice;
        }
        else if (DateAdded.DaysAgo(30))
        {
          MonthPrice = CurrentPrice;
        }
        else if (DateAdded.DaysAgo(90))
        {
          ThreeMonthPrice = CurrentPrice;
        }
        else if (DateAdded.DaysAgo(365))
        {
          YearPrice = CurrentPrice;
        }
      }
      catch { }
    }

    public string DateAndTicker()
    {
      return $"{DateAdded?.ToString("MM/dd/yyyy")}{Ticker}";
    }

    public override bool Equals(object other)
    {
      return DateAndTicker().Equals(((ScreenerRowData)other).DateAndTicker());
    }

    public override int GetHashCode()
    {
      return DateAndTicker().GetHashCode();
    }

  }
}
