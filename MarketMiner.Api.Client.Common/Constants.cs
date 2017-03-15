namespace MarketMiner.Api.Client.Common
{
   public class Constants
   {
      public class SignalSide
      {
         public const string Buy = "B";
         public const string Sell = "S";
         public const string None = "N";
      }

      public class SignalType
      {  
         public const string HeadAndShoulders = "HNS";
         public const string InsideDay = "IDAY";
         public const string OutsideDay = "ODAY";
         public const string PriceGap = "GAP";
         public const string RailRoadTrack = "RRT";
         public const string Thrust = "THRUST";
      }

      public class FibonacciRetracementLevel
      {
         public const double R0 = 0.00;
         public const double R236 = 0.236;
         public const double R382 = 0.382;
         public const double R500 = 0.500;
         public const double R618 = 0.618;
         public const double R764 = 0.764;
      }

      public class TransactionTypes
      {
         public const string LimitOrderCreate = "LIMIT_ORDER_CREATE";
         public const string MarketIfTouchedOrderCreate = "MARKET_IF_TOUCHED_ORDER_CREATE";
         public const string MarketOrderCreate = "MARKET_ORDER_CREATE";
         public const string OrderCancel = "ORDER_CANCEL";
         public const string OrderFilled = "ORDER_FILLED";
         public const string OrderUpdate = "ORDER_UPDATE";
         public const string StopLossFilled = "STOP_LOSS_FILLED";
         public const string TakeProfitFilled = "TAKE_PROFIT_FILLED";
         public const string TradeClose = "TRADE_CLOSE";
         public const string TradeUpdate = "TRADE_UPDATE";
      }
   }
}
