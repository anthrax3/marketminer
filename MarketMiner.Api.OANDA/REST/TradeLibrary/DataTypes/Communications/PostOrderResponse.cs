using System.Collections.Generic;

namespace MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications
{
   public class PostOrderResponse : Response
	{
		public string instrument { get; set; }
		public string time { get; set; }
		public double? price { get; set; }

		public Order orderOpened { get; set; }
		public TradeData tradeOpened { get; set; }
		public List<Transaction> tradesClosed { get; set; } // TODO: verify this
		public Transaction tradeReduced { get; set; } // TODO: verify this
	}
}
