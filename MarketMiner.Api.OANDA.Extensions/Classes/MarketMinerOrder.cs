using MarketMiner.Api.Common.Contracts;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using P.Core.Common.Utils;

namespace MarketMiner.Api.OANDA.Extensions.Classes
{
   public class MarketMinerOrder : Order
   {
      public MarketMinerOrder()
      {
      }

      public MarketMinerOrder(IOrder order)
      {
         this.InjectWith(order);
      }

      public int? SignalID { get; set; }
      public int? StrategyTransactionID { get; set; }

      public bool Filled { get; set; }
      public bool Cancelled { get; set; }
   }
}
