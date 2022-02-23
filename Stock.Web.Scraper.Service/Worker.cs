using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Web.Scraper.Service.Objects;
using Stock.Web.Scraper.Service.ValuesForScraping;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Web.Scraper.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var stock = new YahooStock("YNDX");

                stock.SetPriceToOpen();

                var YandexOpen = stock.Price;

                //UpdateScreener()
                ScraperInfo.Screeners.ForEach(s =>
                {
                    var unscrapedScreener = new YahooStockScreener(s);
                });


                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
