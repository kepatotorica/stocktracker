using Newtonsoft.Json.Linq;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stock.Web.Scraper.Service.Utilities
{
  public class ScrapeScreenerData
  {
    public string screenerCsvPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\Screeners\\";
    public string summaryCsvPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\";
    public string screenerJsonPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\Screeners.json";
    public List<ScreenerTable> screeners = new List<ScreenerTable>();

    public void ScrapeAndUpdate()
    {
      var screenerInfo = JArray.Parse(File.ReadAllText(screenerJsonPath)).Select(jsonItem => jsonItem.ToObject<ScreenerInfo>()).ToList();

      screenerInfo.ForEach(UpdateCsv);
      var summaries = screeners.Select(screener => new SummaryRow(screener)).OrderByDescending(summary => summary.DayPercent);
      UpdateScreenerSummaryCSV(summaries);
    }

    private void UpdateCsv(ScreenerInfo s)
    {
      try
      {
        var screener = new ScreenerTable(s).ScrapeCurrentScreenerData();
        screener.AddRows(GetExistingRows(s));
        screeners.Add(screener);
        UpdateScreenerCsv(screener);
      }
      catch { }
    }

    private IEnumerable<StockRow> GetExistingRows(ScreenerInfo s)
    {
      var stocks = new List<StockRow>();
      try
      {
        stocks = $"{screenerCsvPath}{s.Title}".ReadFromCsv<StockRow>().ToList();
        stocks.ForEach(row => row.UpdatePrices());
      }
      catch { }
      return stocks;
    }

    private void UpdateScreenerCsv(ScreenerTable obj)
    {
      obj.Stocks.WriteToCsv($"{screenerCsvPath}{obj.Title}");
    }

    private void UpdateScreenerSummaryCSV(IEnumerable<SummaryRow> screenerSummaries)
    {
      screenerSummaries.WriteToCsv($"{summaryCsvPath}Summary");
    }
  }
}