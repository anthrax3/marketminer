namespace MarketMiner.Api.Client.Common.Charting
{
   public interface IStudy
   {
      StudyType Type { get; set; }
      int Ordinal { get; set; }

      //int? FillZoneReachedIndex { get; set; }
      //int? TakeProfitZoneReachedIndex { get; set; }
   }

   public enum StudyType
   {
      BollingerBands,
      EquidistantChannel,
      FibonacciExpansion,
      FibonacciRetracement,
      RegressionChannel
   };
}
