using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.Utilities.Csv;
using Stock.Web.Scraper.Service.ValuesForScraping;
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
                var screenersWithData = ScraperInfo.Screeners.Select(s => new YahooStockScreener(s));

                screenersWithData.ToList().ForEach(UpdateCsv);

                await Task.Delay(1000, stoppingToken);
            }
        }

        private void UpdateCsv(YahooStockScreener obj)
        {
            obj.Stocks.WriteToCsv($"{csvPath}{obj.Title}");
            //obj.Stocks.AppendToCsv($"{csvPath}{obj.Title}");
        }
    }
}
