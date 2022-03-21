using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.Utilities;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Web.Scraper.Service
{
  public class ScraperService : BackgroundService
  {
    //public string csvPath = ".\\CsvFiles\\";
    public string csvPath = "C:\\WebScrapin\\stocktracker\\Stock.Web.Scraper.Service\\CsvFiles\\";
    private readonly ILogger<ScraperService> _logger;

    public ScraperService(ILogger<ScraperService> logger)
    {
      _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      CustomTaskScheduler.Instance.ScheduleTask(9, 30, 24, () =>
      {
        Console.WriteLine("Scraping Stonk data" + DateTime.Now);
      });

      CustomTaskScheduler.Instance.ScheduleTask(9, 30, 0.00417, () =>
      {
        Console.WriteLine("This should Happen every 15 seconds" + DateTime.Now);
      });

      while (!stoppingToken.IsCancellationRequested)
      {
        ScraperInfo.Screeners.ForEach(UpdateCsv);
      }
    }

    private void UpdateCsv((string title, string url) s)
    {
      try
      {
        var screener = new FinVizStockScreenerData(s).UpdateScreener();
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

    private void UpdateCsv(FinVizStockScreenerData obj)
    {
      obj.Stocks.WriteToCsv($"{csvPath}{obj.Title}");
    }
  }
}
