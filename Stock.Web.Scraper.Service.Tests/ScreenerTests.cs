using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.Utilities;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Stock.Web.Scraper.Service.Tests
{
  public class ScreenerTests
  {
    public static string now = DateTime.UtcNow.ToString("MM/dd/yy");
    public static DateTime nowDate = DateTime.Parse(now);
    public static DateTime oldestTime = nowDate.AddDays(-365);
    public readonly List<StockRow> stocks;
    public readonly ScreenerTable sut;


    public List<StockRow> ScrapedValues = new List<StockRow>
    {
      new StockRow
      {
        CurrentPrice = 1,
        DateAdded = now,
        PriceWhenAdded = 1,
        Ticker = "KEPA",
      },
      new StockRow
      {
        CurrentPrice = 1,
        DateAdded = now,
        PriceWhenAdded = 1,
        Ticker = "FITRI",
      }
    };

    public List<StockRow> CsvReadValues = new List<StockRow>
    {
      new StockRow
      {
        CurrentPrice = 2,
        DateAdded = nowDate.AddDays(-1).ToString("MM/dd/yy"),
        PriceWhenAdded = 2,
        Ticker = "KEPA",
      },

      new StockRow
      {
        CurrentPrice = 2,
        DateAdded = oldestTime.ToString("MM/dd/yy"),
        PriceWhenAdded = 2,
        Ticker = "JULES",
      }
    };

    public ScreenerTests()
    {
      stocks = ScrapedValues;

      var info = new ScreenerInfo
      {
        Title = "Kepa's Screener",
        Url = "https://finviz.com/screener.ashx?v=111&f=exch_nasd,fa_pe_u10,sh_opt_option,ta_change_d7&ft=4"
      };

      sut = new ScreenerTable(info);
      sut.Stocks = stocks;
    }

    [Fact]
    public void ItProperlyAddsTogetherRows()
    {
      sut.AddRows(CsvReadValues);

      Assert.Equal(260, sut.Stocks.Count());
    }

    [Fact]
    public void TheSummaryIncludesDaysWhereThereAreNoStocks()
    {
      sut.AddRows(CsvReadValues);

      var summaryRow = new SummaryRow(sut);
      var daysSinceOldestDay = oldestTime.DaysSince() + 1;
      Assert.Equal(4m / daysSinceOldestDay, summaryRow.StocksPerDay);
    }
  }
}
