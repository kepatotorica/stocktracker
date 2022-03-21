using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.Utilities.Csv;
using Stock.Web.Scraper.Service.ValuesForScraping;
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
                catch { }
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void UpdateCsv((string title, string url) s)
        {
            var screener = new FinVizStockScreener(s).UpdateScreener();
            screener.AddRows(GetExistingRows(s));
            UpdateCsv(screener);
        }

        private IEnumerable<ScreenerRowData> GetExistingRows((string title, string url) s)
        {
            var stocks = $"{csvPath}{s.title}".ReadFromCsv<ScreenerRowData>().ToList();
            stocks.ForEach(row => row.UpdatePrices());

            return stocks ?? new List<ScreenerRowData>();
        }

        private void UpdateCsv(FinVizStockScreener obj)
        {
            obj.Stocks.WriteToCsv($"{csvPath}{obj.Title}");
        }
    }
}
