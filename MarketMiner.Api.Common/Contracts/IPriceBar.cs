using System;

namespace MarketMiner.Api.Common.Contracts
{
   public interface IPriceBar
   {
      string time { get; set; }
      int volume { get; set; }
      bool complete { get; set; }

      // Midpoint candles
      double openMid { get; set; }
      double highMid { get; set; }
      double lowMid { get; set; }
      double closeMid { get; set; }

      // Bid/Ask candles
      double openBid { get; set; }
      double highBid { get; set; }
      double lowBid { get; set; }
      double closeBid { get; set; }
      double openAsk { get; set; }
      double highAsk { get; set; }
      double lowAsk { get; set; }
      double closeAsk { get; set; }
   }
}
