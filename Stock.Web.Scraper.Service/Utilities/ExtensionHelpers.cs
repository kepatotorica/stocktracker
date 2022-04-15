using System;

namespace Stock.Web.Scraper.Service.Utilities
{
  public static class ExtensionHelpers
  {
    public static bool DaysAgo(this DateTime time, int days)
    {
      //23 and 25 are to give an hour of leway for the run time of the task.
      var utcNow = DateTime.UtcNow;
      var bottomRange = time.AddHours(-1 + 24 * days);
      var topRange = time.AddHours(25 + 24 * days);
      return utcNow > bottomRange && utcNow < topRange;
    }

    public static bool DaysAgo(this DateTime? time, int days)
    {
      return ((DateTime)time).DaysAgo(days);
    }
  }
}