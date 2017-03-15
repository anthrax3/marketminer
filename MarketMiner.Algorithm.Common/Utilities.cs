using System;

namespace MarketMiner.Algorithm.Common
{
   public class Utilities
   {
      public static string GetMissedTradeReason(int tradeCapacity)
      {
         switch (tradeCapacity)
         {
            case Constants.MissedTradeReason.AccountOpenOrder:
               return "Account has open order.";
            case Constants.MissedTradeReason.AccountOpenTrade:
               return "Account has open trade.";
            case Constants.MissedTradeReason.AccountZeroBalance:
               return "Account has zero balance.";
            case Constants.MissedTradeReason.AccountInsufficientMargin:
               return "Account has insufficient margin.";
            default:
               throw new ArgumentException("Unrecognized missed trade reason code.");
         }
      }
   }
}
