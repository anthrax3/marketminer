namespace MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes
{
	public class RateStreamResponse : IHeartbeat
	{
		public Heartbeat heartbeat;
		public Price tick;
		public bool IsHeartbeat()
		{
			return (heartbeat != null);
		}
	}
}
