namespace MarketMiner.Api.Common.Contracts
{
   public interface IIndicator
   {
      IndicatorType Type { get; set; }
      double? Value { get; set; }
   }

   public enum IndicatorType
   {
      SimpleMovingAverage,
      ExponentialMovingAverage,
      DisplacedMovingAverage
   };
}
