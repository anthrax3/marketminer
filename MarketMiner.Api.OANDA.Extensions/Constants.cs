using System;

namespace MarketMiner.Api.OANDA.Extensions
{
   public class Constants
   {
      public static class Instruments
      {
         public const string AUDUSD = "AUD_USD";
         public const string AUDJPY = "AUD_JPY";
         public const string EURUSD = "EUR_USD";
         public const string GBPUSD = "GBP_USD";
         public const string NZDUSD = "NZD_USD";
         public const string USDCHF = "USD_CHF";
         public const string USDCAD = "USD_CAD";
         public const string USDJPY = "USD_JPY";
      }

      public static class Granularity
      {
         public const string Second05 = "S5";
         public const string Second10 = "S10";
         public const string Second15 = "S15";
         public const string Second30 = "S30";
         public const string Minute01 = "M1";
         public const string Minute02 = "M2";
         public const string Minute03 = "M3";
         public const string Minute04 = "M4";
         public const string Minute05 = "M5";
         public const string Minute10 = "M10";
         public const string Minute15 = "M15";
         public const string Minute30 = "M30";
         public const string Hour01 = "H1";
         public const string Hour02 = "H2";
         public const string Hour03 = "H3";
         public const string Hour04 = "H4";
         public const string Hour06 = "H6";
         public const string Hour08 = "H8";
         public const string Hour12 = "H12";
         public const string Day = "D";
         public const string Week = "W";
         public const string Month = "M";
      }

      public static readonly Tuple<string, long>[] Coefficients =   
      {   
          Tuple.Create(Granularity.Second05, (long)1000 * 5),
          Tuple.Create(Granularity.Second10, (long)1000 * 10),
          Tuple.Create(Granularity.Second15, (long)1000 * 15),
          Tuple.Create(Granularity.Second30, (long)1000 * 30),
          Tuple.Create(Granularity.Minute01, (long)1000 * 60),
          Tuple.Create(Granularity.Minute02, (long)1000 * 60 * 2),
          Tuple.Create(Granularity.Minute03, (long)1000 * 60 * 3),
          Tuple.Create(Granularity.Minute04, (long)1000 * 60 * 4),
          Tuple.Create(Granularity.Minute05, (long)1000 * 60 * 5),
          Tuple.Create(Granularity.Minute10, (long)1000 * 60 * 10),
          Tuple.Create(Granularity.Minute15, (long)1000 * 60 * 15),
          Tuple.Create(Granularity.Minute30, (long)1000 * 60 * 30),
          Tuple.Create(Granularity.Hour01, (long)1000 * 60 * 60),
          Tuple.Create(Granularity.Hour02, (long)1000 * 60 * 60 * 2),
          Tuple.Create(Granularity.Hour03, (long)1000 * 60 * 60 * 3),
          Tuple.Create(Granularity.Hour04, (long)1000 * 60 * 60 * 4),
          Tuple.Create(Granularity.Hour06, (long)1000 * 60 * 60 * 6),
          Tuple.Create(Granularity.Hour08, (long)1000 * 60 * 60 * 8),
          Tuple.Create(Granularity.Hour12, (long)1000 * 60 * 60 * 12),
          Tuple.Create(Granularity.Day, (long)1000 * 60 * 60 * 24),
          Tuple.Create(Granularity.Week, (long)1000 * 60 * 60 * 24 * 7),
          Tuple.Create(Granularity.Month, (long)1000 * 60 * 60 * 24 * 30)
      };

      //public class TransactionTypes
      //{
      //   public const string LimitOrderCreate = "LIMIT_ORDER_CREATE";
      //   public const string MarketIfTouchedOrderCreate = "MARKET_IF_TOUCHED_ORDER_CREATE";
      //   public const string MarketOrderCreate = "MARKET_ORDER_CREATE";
      //   public const string OrderCancel = "ORDER_CANCEL";
      //   public const string OrderFilled = "ORDER_FILLED";
      //   public const string OrderUpdate = "ORDER_UPDATE";
      //   public const string StopLossFilled = "STOP_LOSS_FILLED";
      //   public const string TakeProfitFilled = "TAKE_PROFIT_FILLED";
      //   public const string TradeClose = "TRADE_CLOSE";
      //   public const string TradeUpdate = "TRADE_UPDATE";
      //}

      public class OrderCancelReasons
      {
         public const string ClientRequest = "CLIENT_REQUEST";
         public const string TimeInForceExpired = "TIME_IN_FORCE_EXPIRED";
         public const string OrderFilled = "ORDER_FILLED";
         public const string Migration = "MIGRATION";
      }
   }
}
