using Stock.Web.Scraper.Service.Utilities;

namespace Stock.Web.Scraper.Service
{
  public class Program
  {
    public static void Main(string[] args)
    {
      new ScrapeScreenerData().ScrapeAndUpdate();
    }
  }
}
