using Stock.Web.Scraper.Service.Utilities;
using System;
using Xunit;

namespace Stock.Web.Scraper.Service.Tests
{
  public class ExtensionHelpersTests
  {
    public static DateTime now = DateTime.Now;
    public static DateTime yesterday = DateTime.Now.AddDays(-1);

    [Theory]
    [InlineData(1, "4/15/2022", 0)]
    [InlineData(2, "4/15/2022", 0)]
    [InlineData(3, "4/15/2022", 1)]
    [InlineData(7, "4/15/2022", 5)]
    [InlineData(14, "4/15/2022", 10)]
    public void ItCalculatesTheNumberOfBusinessDays(int daysSince, string startDate, int expected)
    {
      var daysBetween = DateTime.Parse(startDate).AddDays(daysSince).BusinessDaysBetween(now);
      Assert.Equal(expected, daysBetween);
    }
  }
}
