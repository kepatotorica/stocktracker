using System;

namespace Stock.Web.Scraper.Service.Utilities
{
  public static class ExtensionHelpers
  {
    public static bool DaysAgo(this DateTime time, int days)
    {
      //23 and 25 are to give an hour of leway for the run time of the task.
      var utcNow = DateTime.UtcNow;
      var bottomRange = utcNow.AddHours(23 + 24 * days);
      var topRange = utcNow.AddHours(25 + 24 * days);
      return time > topRange && time < bottomRange;
    }

    public static bool DaysAgo(this DateTime? time, int days)
    {
      return ((DateTime)time).DaysAgo(days);
    }
  }
}