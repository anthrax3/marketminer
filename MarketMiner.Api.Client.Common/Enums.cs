namespace MarketMiner.Api.Client.Common
{
   public enum ApiConnectionStatus
   {
      Connected,
      Streaming,
      Faulted,
      Disconnected,
      Recovering,
      Recovered
   }

   public enum ApiStreamStatus
   {
      RatesStarted,
      RatesStopped,
      EventsStarted,
      EventsStopped
   }

   public enum EPatternDirection
   {
      Up,
      Down,
      None
   }
}
