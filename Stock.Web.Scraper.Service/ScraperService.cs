using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.Utilities;
using Stock.Web.Scraper.Service.Utilities.Csv;
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
        public string csvPath = ".\\CsvFiles\\";
        private readonly ILogger<ScraperService> _logger;

        public ScraperService(ILogger<ScraperService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ScraperInfo.Screeners.ForEach(UpdateCsv);
                }
                finally
                {
                    CustomTaskScheduler.Instance.ScheduleTask(9, 30, 24, () =>
                    {
                        Console.WriteLine("Scraping Stonk data" + DateTime.Now);
                        //here write the code that you want to schedule
                    });

                    CustomTaskScheduler.Instance.ScheduleTask(9, 30, 0.00417, () =>
                    {
                        Console.WriteLine("This should Happen every 15 seconds" + DateTime.Now);
                        //here write the code that you want to schedule
                    });
                }
            }
        }

        private void UpdateCsv((string title, string url) s)
        {
            var screener = new FinVizStockScreener(s).UpdateScreener();
            screener.AddRows(GetExistingRows(s));
            UpdateCsv(screener);
        }

        private IEnumerable<ScreenerRow> GetExistingRows((string title, string url) s)
        {
            var stocks = $"{csvPath}{s.title}".ReadFromCsv<ScreenerRow>().ToList();
            stocks.ForEach(row => row.UpdatePrices());

            return stocks ?? new List<ScreenerRow>();
        }

        private void UpdateCsv(FinVizStockScreener obj)
        {
            obj.Stocks.WriteToCsv($"{csvPath}{obj.Title}");
        }
    }
}
