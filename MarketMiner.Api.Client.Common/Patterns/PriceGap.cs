namespace MarketMiner.Api.Client.Common.Patterns
{
   public class PriceGap : Pattern
   {
      public PriceGap()
      {
         Type = MarketMiner.Api.Client.Common.Constants.SignalType.PriceGap;
      }

      public EPriceGapOccasion Occasion { get; set; }
   }

   public enum EPriceGapOccasion
   {
      MarketOpen,
      Intraday
   }
}
