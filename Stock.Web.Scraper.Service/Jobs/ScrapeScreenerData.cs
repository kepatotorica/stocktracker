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

    public ScrapeScreenerData() { }

    public void RunScrapers()
    {
      ScraperInfo.Screeners.ForEach(UpdateCsv);
    }

    private void UpdateCsv((string title, string url) s)
    {
      try
      {
        var screener = new FinVizStockScreener(s).ScrapeCurrentScreenerData();
        screener.AddRows(GetExistingRows(s));
        UpdateCsv(screener);
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

    private void UpdateCsv(FinVizStockScreener obj)
    {
      obj.Stocks.WriteToCsv($"{csvPath}{obj.Title}");
    }
  }
}