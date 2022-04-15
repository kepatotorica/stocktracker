using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.Utilities;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Web.Scraper.Service.Jobs
{
  public class ScrapeScreenerData
  {
    public string screenerCsvPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\Screeners\\";
    public string summaryCsvPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\";
    public List<Screener> screeners = new List<Screener>();

    public void ScrapeAndUpdate()
    {
      ScraperInfo.Screeners.ForEach(UpdateCsv);
      var summaries = screeners.Select(screener => new ScreenerSummary(screener)).OrderByDescending(summary => summary.DayPercent);
      UpdateScreenerSummaryCSV(summaries);
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
        stocks = $"{screenerCsvPath}{s.title}".ReadFromCsv<ScreenerRowData>().ToList();
        stocks.ForEach(row => row.UpdatePrices());
      }
      catch { }
      return stocks;
    }

    private void UpdateScreenerCsv(Screener obj)
    {
      obj.Stocks.WriteToCsv($"{screenerCsvPath}{obj.Title}");
    }

    private void UpdateScreenerSummaryCSV(IEnumerable<ScreenerSummary> screenerSummaries)
    {
      screenerSummaries.WriteToCsv($"{summaryCsvPath}Summary");
    }
  }
}