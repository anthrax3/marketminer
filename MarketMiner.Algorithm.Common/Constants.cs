namespace MarketMiner.Algorithm.Common
{
   public class Constants
   {
      public class MissedTradeReason
      {
         public const short AccountOpenOrder = -1;
         public const short AccountOpenTrade = -2;
         public const short AccountZeroBalance = -3;
         public const short AccountInsufficientMargin = -4;
         public const short InvalidBuyOrderEntryGreaterThanTakeProfit = -96;
         public const short InvalidBuyOrderEntryLessThanStopLoss = -97;
         public const short InvalidSellOrderEntryLessThanTakeProfit = -98;
         public const short InvalidSellOrderEntryGreaterThanStopLoss = -99;
      }
   }
}
