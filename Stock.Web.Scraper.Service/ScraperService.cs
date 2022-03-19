using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.Utilities.Csv;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System;
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
                UpdateAndScrapeAllScreeners();

                await Task.Delay(1000, stoppingToken);
            }
        }

        private void UpdateAndScrapeAllScreeners()
        {
            var screenersWithData = ScraperInfo.Screeners.Select(s => new FinVizStockScreener(s));
            screenersWithData = screenersWithData.Select(s => (UpdateScreenerWithExistingCsvData(s)));
            var MyStocks = screenersWithData.First().Stocks.ToList();
            screenersWithData.ToList().ForEach(UpdateCsv);
        }

        private FinVizStockScreener UpdateScreenerWithExistingCsvData(FinVizStockScreener s)
        {
            try
            {
                var screener = $"{csvPath}{s.Title}".ReadFromCsv<YahooStock>().ToList();
                s.Stocks = s.Stocks.Concat(screener).ToList();
                //TODOASDF make these add together somehow, instead of them overwriting eachother
                return s;
            }
            catch (Exception ex)
            {
                return s;
            }
        }

        private void UpdateCsv(FinVizStockScreener obj)
        {
            obj.Stocks.WriteToCsv($"{csvPath}{obj.Title}");
            //obj.Stocks.AppendToCsv($"{csvPath}{obj.Title}");
        }
    }
}
