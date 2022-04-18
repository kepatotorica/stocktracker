namespace Stock.Web.Scraper.Service.ValuesForScraping
{
  public static class ScraperXpaths
  {
    //https://www.w3schools.com/xml/xpath_syntax.asp
    public static class StockPageIds
    {
      public static string CurrentValue => "/html/body/div[4]/div/table[2]/tr[last()-1]/td[last()]/b";
    }

    public static class ScreenerPageIds
    {
      public static string RowNames => "//table//tr//td//a[contains(@class, 'screener-link-primary')]";
      public static string RowPrice => "//table//tr//td//a//span";
      public static string RowPriceWithTicker(string ticker) => $"//table//tr//td//a[contains(@href, 'quote.ashx?t={ticker}&ty=c&p=d&b=1')]";
    };
  }
}
