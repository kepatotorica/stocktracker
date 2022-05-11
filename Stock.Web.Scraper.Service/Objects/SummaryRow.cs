using Stock.Web.Scraper.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Web.Scraper.Service.Objects
{
  public class SummaryRow
  {
    public string ScreenerName { get; set; }
    public decimal StocksPerDay { get; set; }
    public decimal? DayPercent { get; set; }
    public decimal? FiveDayPercent { get; set; }
    public decimal? TenDayPercent { get; set; }
    public decimal? MonthPercent { get; set; }
    public decimal? ThreeMonthPercent { get; set; }
    public decimal? YearPercent { get; set; }

    public SummaryRow(ScreenerTable screener)
    {
      var daysScreened = screener.Stocks
        .Select(s => DateTime.Parse(s.DateAdded))
        .Distinct()
        .OrderBy(d => d.Year)
        .ThenBy(d => d.Month)
        .ThenBy(d => d.Day)
        .First().DaysSince() + 1m;
      ScreenerName = screener.Title;
      StocksPerDay = screener.Stocks.Any() ? screener.Stocks.Count() / daysScreened : 0m;
      UpdatePercentages(screener.Stocks);
    }

    private void UpdatePercentages(List<StockRow> stocks)
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
