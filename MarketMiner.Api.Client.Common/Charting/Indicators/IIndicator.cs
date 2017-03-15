using P.Core.Common.Core;
using System.Collections.Generic;

namespace MarketMiner.Api.Client.Common.Charting
{
   public interface IIndicator
   {
      IndicatorType Type { get; set; }
      double? Value { get; set; }
      short Period { get; set; }
      int Ordinal { get; set; }
      int ParentOrdinal { get; set; }

      List<IIndicator> SetValue(IChart chart);
      List<IIndicator> SetValue(IChart chart, int childOrdinalStart);
   }

   public enum IndicatorType
   {
      DetrendedOscillator,
      DisplacedMovingAverage,
      ExponentialMovingAverage,
      MACD,           
      SimpleMovingAverage
   };
}
