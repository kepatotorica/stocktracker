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
    public decimal CurrentPrice { get; set; }
    public decimal PriceWhenAdded { get; set; }
    public string DateAdded { get; set; }
    public decimal? DayPercent { get; set; }
    public decimal? FiveDayPercent { get; set; }
    public decimal? TenDayPercent { get; set; }
    public decimal? MonthPercent { get; set; }
    public decimal? ThreeMonthPercent { get; set; }
    public decimal? YearPercent { get; set; }

    public decimal? GetOpenPrice()
    {
      HtmlDocument doc = new HtmlWeb().Load($"https://finance.yahoo.com/quote/{Ticker}?p={Ticker}");

      return Decimal.Parse(doc.DocumentNode.SelectNodes(ScraperInfo.StockPageIds.Open).First().InnerHtml);
    }

    public decimal GetCurrentPrice()
    {
      HtmlDocument doc = new HtmlWeb().Load($"https://finance.yahoo.com/quote/{Ticker}?p={Ticker}");

      return Decimal.Round(Decimal.Parse(doc.DocumentNode.SelectNodes(ScraperInfo.StockPageIds.CurrentValue).First().InnerHtml), 2);
    }

    public void UpdatePrices()
    {
      //TODOASDF Find a way to cache all of the prices for any stock so we don't have to keep scraping data if the same stock pops up
      try
      {
        var now = DateTime.Now;
        var dateAdded = DateTime.Parse(DateAdded);

        if (DateAdded != now.ToString("MM/dd/yy"))
        {
          CurrentPrice = GetCurrentPrice();
        }

        if (dateAdded.BusinessDaysBetween(now) == 1)
        {
          DayPercent = PercentChangedSinceAdded();
        }
        else if (dateAdded.BusinessDaysBetween(now) == 5)
        {
          FiveDayPercent = PercentChangedSinceAdded();
        }
        else if (dateAdded.BusinessDaysBetween(now) == 10)
        {
          TenDayPercent = PercentChangedSinceAdded();
        }
        else if (dateAdded.BusinessDaysBetween(now) == 30)
        {
          MonthPercent = PercentChangedSinceAdded();
        }
        else if (dateAdded.BusinessDaysBetween(now) == 90)
        {
          ThreeMonthPercent = PercentChangedSinceAdded();
        }
        else if (dateAdded.BusinessDaysBetween(now) == 365)
        {
          YearPercent = PercentChangedSinceAdded();
        }
      }
      catch { }
    }

    private decimal? PercentChangedSinceAdded()
    {
      return Decimal.Round(((CurrentPrice - PriceWhenAdded) / PriceWhenAdded * 100), 2);
    }

    public string DateAndTicker()
    {
      return $"{DateAdded}{Ticker}";
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
