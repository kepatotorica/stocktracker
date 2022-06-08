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

    public SummaryRow(ScreenerTableScraper screener)
    {
      if (screener.Stocks.Any())
      {
        var daysScreened = screener.Stocks
          .Select(s => DateTime.Parse(s.DateAdded))
          .Distinct()
          .OrderBy(d => d.Year)
          .ThenBy(d => d.Month)
          .ThenBy(d => d.Day)
          .First().DaysSince() + 1m;
        ScreenerName = screener.Title;
        StocksPerDay = screener.Stocks.Any() ? Decimal.Round(screener.Stocks.Count() / daysScreened, 2) : 0m;
        UpdatePercentages(screener.Stocks);
      }
    }

    private void UpdatePercentages(List<StockRow> stocks)
    {
      DayPercent = stocks.Select(s => s.DayPercent).AveragePercent();
      FiveDayPercent = stocks.Select(s => s.FiveDayPercent).AveragePercent();
      TenDayPercent = stocks.Select(s => s.TenDayPercent).AveragePercent();
      MonthPercent = stocks.Select(s => s.MonthPercent).AveragePercent();
      ThreeMonthPercent = stocks.Select(s => s.ThreeMonthPercent).AveragePercent();
      YearPercent = stocks.Select(s => s.YearPercent).AveragePercent();
    }
  }
}
