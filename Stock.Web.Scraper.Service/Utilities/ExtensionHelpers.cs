using System;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Web.Scraper.Service.Utilities
{
  public static class ExtensionHelpers
  {
    public static decimal AveragePercent(this IEnumerable<decimal?> percents)
    {
      return Decimal.Round(percents.Where(percent => percent != null).Average() ?? 0m, 2);
    }

    public static int DaysSince(this DateTime startDate)
    {
      return startDate.BusinessDaysBetween(DateTime.UtcNow);
    }

    public static int BusinessDaysBetween(this DateTime startDate, DateTime endDate)
    {
      var years = Enumerable.Range(startDate.Year, endDate.Year);
      var holidays = GetHolidays(years);

      int numberOfBusinessDays = 0;
      for (var current = startDate.AddDays(1); current <= endDate; current = current.AddDays(1))
      {
        if (!(current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday || (holidays != null && holidays.Contains(current.Date))))
        {
          numberOfBusinessDays++;
        }
      }
      return numberOfBusinessDays;
    }

    public static IEnumerable<DateTime> GetHolidays(IEnumerable<int> years, string countryCode = null, string cityName = null)
    {
      var listOfDates = new List<DateTime>();

      foreach (var year in years.Distinct())
      {
        listOfDates.AddRange(new[] {
                new DateTime(year, 1, 1),
                new DateTime(year, 1, 6),
                new DateTime(year, 5, 1),
                new DateTime(year, 8, 15),
                new DateTime(year, 11, 1),
                new DateTime(year, 12, 8),
                new DateTime(year, 12, 25),
                new DateTime(year, 12, 26)
            });

        if (!String.IsNullOrEmpty(countryCode))
        {
          switch (countryCode.ToUpper())
          {
            case "IT":
              listOfDates.Add(new DateTime(year, 4, 25));
              break;
            case "US":
              listOfDates.Add(new DateTime(year, 7, 4));
              break;
            default:
              break;
          }
        }

        if (!String.IsNullOrEmpty(cityName))
        {
          switch (cityName)
          {
            case "Rome":
            case "Roma":
              listOfDates.Add(new DateTime(year, 6, 29));
              break;
            case "Milano":
            case "Milan":
              listOfDates.Add(new DateTime(year, 12, 7));
              break;

            default:
              break;
          }
        }
      }
      return listOfDates;
    }


  }
}