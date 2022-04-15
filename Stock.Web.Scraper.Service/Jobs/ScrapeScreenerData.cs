using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.Utilities;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Web.Scraper.Service.Jobs
{
  public class ScrapeScreenerData
  {
    public string csvPath = "C:\\Users\\kep\\Desktop\\EasyAccess\\CSV Files\\";
    public List<Screener> screeners = new List<Screener>();

    public void ScrapeAndUpdate()
    {
      ScraperInfo.Screeners.ForEach(UpdateCsv);
      var summaries = screeners.Select(screener => new ScreenerSummary(screener)).OrderByDescending(summary => summary.DayPercent);
      UpdateScreenerSummaryCSV(summaries);
    }

    private void UpdateScreenerSummaryCSV(IEnumerable<ScreenerSummary> screenerSummaries)
    {
      screenerSummaries.WriteToCsv($"{csvPath}Summary");
    }

    private void UpdateCsv((string title, string url) s)
    {
      try
      {
        var screener = new Screener(s).ScrapeCurrentScreenerData();
        screener.AddRows(GetExistingRows(s));
        screeners.Add(screener);
        UpdateScreenerCsv(screener);
      }
      catch { }
    }

    private IEnumerable<ScreenerRowData> GetExistingRows((string title, string url) s)
    {
      var stocks = new List<ScreenerRowData>();
      try
      {
        stocks = $"{csvPath}{s.title}".ReadFromCsv<ScreenerRowData>().ToList();
        stocks.ForEach(row => row.UpdatePrices());
      }
      catch { }
      return stocks;
    }

    private void UpdateScreenerCsv(Screener obj)
    {
      obj.Stocks.WriteToCsv($"{csvPath}{obj.Title}");
    }
  }
}