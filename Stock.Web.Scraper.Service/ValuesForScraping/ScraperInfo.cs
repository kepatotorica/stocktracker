using System.Collections.Generic;

namespace Stock.Web.Scraper.Service.ValuesForScraping
{
    public static class ScraperInfo
    {
        public static class StockPageIds
        {
            public static string Open => "//td[@data-test='OPEN-value']";
        }

        public static class ScreenerPageIds
        {
            public static string RowNames => "//tbody/tr/td/a";
            public static string RowPrice => "//tbody/tr/td/fin-streamer[@data-field='regularMarketPrice']";
        };

        public static readonly List<(string title, string url)> Screeners = new()
        {
            new("Specific Testing Screener", "https://finance.yahoo.com/screener/unsaved/9ae09b48-c915-4b5d-b040-17bef5e49a81?dependentField=sector&dependentValues="),
            new("Big Drop Large Cap", "https://finance.yahoo.com/screener/unsaved/894b6531-8650-4386-9038-86663c38712f"),
        };
    }
}
