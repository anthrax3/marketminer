using MarketMiner.Api.Common.Contracts;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using P.Core.Common.Utils;

namespace MarketMiner.Api.OANDA.Extensions.Classes
{
   public class MarketMinerTrade : TradeData
   {
      public MarketMinerTrade()
      {
      }

      public MarketMinerTrade(ITrade trade)
      {
         this.InjectWith(trade);
      }

      public int? SignalID { get; set; }
      public int? StrategyTransactionID { get; set; }

      public bool Closed { get; set; }
   }
}
