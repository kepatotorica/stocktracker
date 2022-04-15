﻿using System.Collections.Generic;

namespace Stock.Web.Scraper.Service.ValuesForScraping
{
  public static class ScraperInfo
  {
    public static class StockPageIds
    {
      public static string Open => "//td[@data-test='OPEN-value']";

      public static string CurrentValue => "//*[@id='quote-header-info']/div[3]/div[1]/div[1]/fin-streamer[1]";
    }

    public static class ScreenerPageIds
    {
      public static string RowNames => "//table//tr//td//a[contains(@class, 'screener-link-primary')]";
      public static string RowPrice => "//table//tr//td//a//span";
      public static string RowPriceWithTicker(string ticker) => $"//table//tr//td//a[contains(@href, 'quote.ashx?t={ticker}&ty=c&p=d&b=1')]";
    };

    public static readonly List<(string title, string url)> Screeners = new()
    {
      new("Big Drop-low PE-Optionable", "https://finviz.com/screener.ashx?v=111&f=exch_nasd,fa_pe_u10,sh_opt_option,ta_change_d7&ft=4"),
      new("Specific Testing Screener", "https://finviz.com/screener.ashx?v=111&f=cap_microunder,exch_nasd,sh_avgvol_u750,sh_short_o30"),
      new("Shorted stocks", "https://finviz.com/screener.ashx?v=131&f=cap_smallover,geo_usa,sh_avgvol_o500,sh_curvol_o500,sh_opt_optionshort,sh_price_o3,sh_relvol_o1,sh_short_high&o=-shortinterestshare"),
      new("Short squeeze", "https://finviz.com/screener.ashx?v=131&f=sh_avgvol_o100,sh_instown_u50,sh_price_o2,sh_short_o15&ft=4&o=-shortinterestshare"),
      new("Weekly Earnings gap up", "https://finviz.com/screener.ashx?v=141&f=earningsdate_tomorrowafter,sh_avgvol_o400,sh_curvol_o50,sh_short_u25,ta_averagetruerange_o0.5,ta_gap_u2&ft=4&o=-perfytd"),
      new("Bankruptcy squeeze candidates", "https://finviz.com/screener.ashx?v=131&f=fa_pb_low,sh_short_o30&ft=4&o=-shortinterestshare"),
      new("Potential uptrend from weekly lows", "https://finviz.com/screener.ashx?v=141&f=sh_avgvol_o400,ta_pattern_channelup,ta_perf_1wdown&ft=4&o=perf1w"),
      new("Bounce at moving average", "https://finviz.com/screener.ashx?v=141&f=sh_avgvol_o400,sh_curvol_o2000,sh_relvol_o1,ta_sma20_pa,ta_sma50_pb&ft=4&o=-perf1w"),
      new("Oversold reversal", "https://finviz.com/screener.ashx?v=111&f=sh_price_o5,sh_relvol_o2,ta_change_u,ta_rsi_os30&ft=4&o=price"),
      new("Oversold with upcoming earnings", "https://finviz.com/screener.ashx?v=141&f=cap_smallover,earningsdate_thismonth,fa_epsqoq_o15,fa_grossmargin_o20,sh_avgvol_o750,sh_curvol_o1000,ta_perf_52w10o,ta_rsi_nob50&ft=4&o=perfytd"),
      new("New highs", "https://finviz.com/screener.ashx?v=141&f=an_recom_buy,sh_price_u7,ta_change_u,ta_highlow20d_nh,ta_highlow50d_nh,ta_highlow52w_nh,ta_perf_dup&ft=4&o=-perf1w"),
      new("Breaking out", "https://finviz.com/screener.ashx?v=141&f=fa_debteq_u1,fa_roe_o20,sh_avgvol_o100,ta_highlow50d_nh,ta_sma20_pa,ta_sma200_pa,ta_sma50_pa&ft=4&o=-perf1w"),
      new("SMA crossover", "https://finviz.com/screener.ashx?v=141&f=fa_pe_profitable,sh_avgvol_o400,sh_relvol_o1,sh_short_low,ta_beta_o1,ta_sma50_cross20b&ft=4"),
      new("High Earnings growth", "https://finviz.com/screener.ashx?v=141&f=fa_epsqoq_o25,fa_epsyoy_o25,fa_epsyoy1_o25,fa_salesqoq_o25,sh_avgvol_o400,ta_rsi_nos50,ta_sma200_pa&ft=4&o=-perfytd"),
      new("High Sales growth", "https://finviz.com/screener.ashx?v=111&f=fa_debteq_u0.5,fa_roe_o15,fa_sales5years_o20,fa_salesqoq_o20,sh_avgvol_o200,sh_instown_o60,sh_price_o5,sh_short_u5&ft=4"),
      new("High relative volume", "https://finviz.com/screener.ashx?v=131&f=fa_curratio_o1,fa_epsqoq_o15,fa_quickratio_o1,fa_salesqoq_o15,sh_avgvol_o400,sh_price_o5,sh_relvol_o1.5,ta_sma20_pa,ta_sma200_sb50,ta_sma50_sa200&ft=4&o=instown"),
      new("Consistent growth on a bullish trend", "https://finviz.com/screener.ashx?v=141&f=fa_eps5years_pos,fa_epsqoq_o20,fa_epsyoy_o25,fa_epsyoy1_o15,fa_estltgrowth_pos,fa_roe_o15,sh_instown_o10,sh_price_o15,ta_highlow52w_a90h,ta_rsi_nos50&ft=4&o=-perfytd"),
      new("Buy and Hold value", "https://finviz.com/screener.ashx?v=121&f=cap_microover,fa_curratio_o1.5,fa_estltgrowth_o10,fa_peg_o1,fa_roe_o15,ta_beta_o1.5,ta_sma20_pa&ft=4&o=-forwardpe"),
      new("Undervalued dividend growth", "https://finviz.com/screener.ashx?v=111&f=cap_largeover,fa_div_pos,fa_epsyoy1_o5,fa_estltgrowth_o5,fa_payoutratio_u50,fa_pe_u20,fa_peg_low&ft=4&o=-pe"),
      new("Low PE value", "https://finviz.com/screener.ashx?v=141&f=cap_smallunder,fa_pb_low,fa_pe_low,fa_peg_low,fa_roa_pos,fa_roe_pos,sh_price_o5&ft=4&o=-perfytd"),
      new("CANSLIM", "https://finviz.com/screener.ashx?v=111&f=fa_eps5years_o20,fa_epsqoq_o20,fa_epsyoy_o20,fa_sales5years_o20,fa_salesqoq_o20,sh_curvol_o200&ft=4"),
    };
  }
}
