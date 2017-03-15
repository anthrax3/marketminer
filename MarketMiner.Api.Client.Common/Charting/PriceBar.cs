using MarketMiner.Api.Common.Contracts;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class PriceBar : IPriceBar
   {
      public string time { get; set; }
      public int volume { get; set; }
      public bool complete { get; set; }
      public double openMid { get; set; }
      public double highMid { get; set; }
      public double lowMid { get; set; }
      public double closeMid { get; set; }
      public double openBid { get; set; }
      public double highBid { get; set; }
      public double lowBid { get; set; }
      public double closeBid { get; set; }
      public double openAsk { get; set; }
      public double highAsk { get; set; }
      public double lowAsk { get; set; }
      public double closeAsk { get; set; }
      //

      public string instrument { get; set; }
      public string granularity { get; set; }
      public double open { get; set; }
      public double high { get; set; }
      public double low { get; set; }
      public double close { get; set; }
   }
}
