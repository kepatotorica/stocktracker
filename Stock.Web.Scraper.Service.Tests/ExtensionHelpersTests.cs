using Stock.Web.Scraper.Service.Utilities;
using System;
using Xunit;

namespace Stock.Web.Scraper.Service.Tests
{
  public class ExtensionHelpersTests
  {
    [Theory]
    [InlineData(0, "04/15/2022", 0)]
    [InlineData(1, "04/15/2022", 0)]
    [InlineData(2, "04/15/2022", 0)]
    [InlineData(3, "04/15/2022", 1)]
    [InlineData(7, "04/15/2022", 5)]
    [InlineData(14, "04/15/2022", 10)]
    public void ItCalculatesTheNumberOfBusinessDays(int daysSince, string startDate, int expected)
    {
      var start = DateTime.Parse(startDate);
      var end = DateTime.Parse(startDate).AddDays(daysSince);
      var daysBetween = start.BusinessDaysBetween(end);
      Assert.Equal(expected, daysBetween);
    }
  }
}
