using Stock.Web.Scraper.Service.Objects;
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
    public readonly List<ScreenerRowData> stocks;
    public readonly Screener sut;


    public List<ScreenerRowData> ScrapedValues = new List<ScreenerRowData>
    {
      new ScreenerRowData
      {
        CurrentPrice = 1,
        DateAdded = now,
        PriceWhenAdded = 1,
        Ticker = "KEPA",
      },
      new ScreenerRowData
      {
        CurrentPrice = 1,
        DateAdded = now,
        PriceWhenAdded = 1,
        Ticker = "FITRI",
      }
    };

    public List<ScreenerRowData> CsvReadValues = new List<ScreenerRowData>
    {
      new ScreenerRowData
      {
        CurrentPrice = 2,
        DateAdded = nowDate.AddDays(-1).ToString("MM/dd/yy"),
        PriceWhenAdded = 2,
        Ticker = "KEPA",
      },

      new ScreenerRowData
      {
        CurrentPrice = 2,
        DateAdded = nowDate.AddDays(-1).ToString("MM/dd/yy"),
        PriceWhenAdded = 2,
        Ticker = "JULES",
      }
    };

    public ScreenerTests()
    {
      stocks = ScrapedValues;

      sut = new Screener(("Kepa's Screener", "https://finviz.com/screener.ashx?v=111&f=exch_nasd,fa_pe_u10,sh_opt_option,ta_change_d7&ft=4"));
      sut.Stocks = stocks;
    }

    [Fact]
    public void ItProperlyAddsTogetherRows()
    {
      sut.AddRows(CsvReadValues);

      Assert.Equal(4, sut.Stocks.Count());
    }
  }
}
