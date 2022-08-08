using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Stock.Web.Scraper.Service.Utilities;
using System;

namespace Stock.Web.Scraper.Service
{
  public class Program
  {
    public static IConfiguration Config { get; private set; }


    public static void Main(string[] args)
    {
      Config = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>()
        .Build();

      new GetAndUpdateAllScreeners(new MemoryCache(new MemoryCacheOptions())).ScrapeAndUpdate();
    }
  }
}
