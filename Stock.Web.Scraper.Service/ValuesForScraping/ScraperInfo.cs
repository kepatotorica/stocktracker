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
            public static string RowNames => "//table//tr//td//a[contains(@class, 'screener-link-primary')]";
            public static string RowPrice => "//table//tr//td//a//span";
        };

        public static readonly List<(string title, string url)> Screeners = new()
        {
            //new("Yahoo", "https://finance.yahoo.com/screener/predefined/growth_technology_stocks"),
            //new("Big Drop-low PE-Optionable", "https://finviz.com/screener.ashx?v=111&f=exch_nasd,fa_pe_u10,sh_opt_option,ta_change_d7&ft=4"),
            new("Specific Testing Screener", "https://finviz.com/screener.ashx?v=111&f=cap_microunder,exch_nasd,sh_avgvol_u750,sh_short_o30"),
        };
    }
}
