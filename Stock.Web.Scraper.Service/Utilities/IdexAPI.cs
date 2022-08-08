using Stock.Web.Scraper.Service.Objects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace Stock.Web.Scraper.Service.Utilities
{
  public static class IexAPI
  {
    public static string APIBase = "https://cloud.iexapis.com/stable/";

    public static IEnumerable<IexQuote> GetTickers(IEnumerable<string> stockTickers)
    {
      var apiKey = Program.Config["IEXApiKey"];
      string QUERY_URL = $"{APIBase}tops?token={apiKey}&symbols={string.Join(",", stockTickers)}";
      Uri queryUri = new Uri(QUERY_URL);

      using (WebClient client = new WebClient())
      {
        dynamic data = JsonSerializer.Deserialize<IEnumerable<IexQuote>>(client.DownloadString(queryUri));

        return data;
      }
    }
  }
}
