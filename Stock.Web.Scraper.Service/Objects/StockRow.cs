using Stock.Web.Scraper.Service.Utilities;
using System;

namespace Stock.Web.Scraper.Service.Objects
{
  public class StockRow
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

    public void UpdatePrices(decimal currentPrice)
    {
      try
      {
        var now = DateTime.Now;
        var dateAdded = DateTime.Parse(DateAdded);

        if (DateAdded != now.ToString("MM/dd/yy"))
        {
          CurrentPrice = currentPrice;
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
      return DateAndTicker().Equals(((StockRow)other).DateAndTicker());
    }

    public override int GetHashCode()
    {
      return DateAndTicker().GetHashCode();
    }
  }
}
