using System.Collections.Generic;
using System.Linq;

namespace Stock.Web.Scraper.Service.Objects
{
  public class ScreenerSummary
  {
    public string ScreenerName { get; set; }
    public decimal StocksPerDay { get; set; }
    public decimal? DayPercent { get; set; }
    public decimal? FiveDayPercent { get; set; }
    public decimal? TenDayPercent { get; set; }
    public decimal? MonthPercent { get; set; }
    public decimal? ThreeMonthPercent { get; set; }
    public decimal? YearPercent { get; set; }

    public ScreenerSummary(Screener screener)
    {
      ScreenerName = screener.Title;
      StocksPerDay = screener.Stocks.Any() ? screener.Stocks.Count() / screener.Stocks.Select(s => s.DateAdded).Distinct().Count() : 0m;
      UpdatePercentages(screener.Stocks);
    }

    private void UpdatePercentages(List<ScreenerRowData> stocks)
    {
      DayPercent = stocks.Select(s => s.DayPercent).Where(percent => percent != null).Average();
      FiveDayPercent = stocks.Select(s => s.FiveDayPercent).Where(percent => percent != null).Average();
      TenDayPercent = stocks.Select(s => s.TenDayPercent).Where(percent => percent != null).Average();
      MonthPercent = stocks.Select(s => s.MonthPercent).Where(percent => percent != null).Average();
      ThreeMonthPercent = stocks.Select(s => s.ThreeMonthPercent).Where(percent => percent != null).Average();
      YearPercent = stocks.Select(s => s.YearPercent).Where(percent => percent != null).Average();
    }
  }
}
