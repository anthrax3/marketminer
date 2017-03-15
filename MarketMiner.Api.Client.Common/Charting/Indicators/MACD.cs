using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class MACD : Indicator
   {
      private const IndicatorType INDICATORTYPE = IndicatorType.MACD;

      short _slowEMAPeriod, _fastEMAPeriod, _signalEMAPeriod;
      public double? Signal { get; set; }
      public double? Histogram { get; set; }

      public MACD(short slowEMAPeriod, short fastEMAPeriod, short signalEMAPeriod)
         : base(INDICATORTYPE)
      {
         _slowEMAPeriod = slowEMAPeriod;
         _fastEMAPeriod = fastEMAPeriod;
         _signalEMAPeriod = signalEMAPeriod;
      }

      public override List<IIndicator> SetValue(IChart chart, int childOrdinalStart)
      {
         Value = null;

         ExponentialMovingAverage slowEMA, fastEMA, signalEMA;
         CreateChildIndicators(chart, childOrdinalStart, out slowEMA, out fastEMA, out signalEMA);

         Value = slowEMA.Value - fastEMA.Value;
         Signal = signalEMA.Value;
         Histogram = Value - Signal;

         return new List<IIndicator>(new IIndicator[] { slowEMA, fastEMA, signalEMA });
      }

      private void CreateChildIndicators(IChart chart, int childOrdinalStart, out ExponentialMovingAverage slowEMA, out ExponentialMovingAverage fastEMA, out ExponentialMovingAverage signalEMA)
      {
         slowEMA = new ExponentialMovingAverage(_slowEMAPeriod) { ParentOrdinal = this.Ordinal, Ordinal = childOrdinalStart + 0 };
         slowEMA.SetValue(chart);

         fastEMA = new ExponentialMovingAverage(_fastEMAPeriod) { ParentOrdinal = this.Ordinal, Ordinal = childOrdinalStart + 1 };
         fastEMA.SetValue(chart);

         double currentMACDValue = (slowEMA.Value - fastEMA.Value).GetValueOrDefault();
         signalEMA = new ExponentialMovingAverage(_signalEMAPeriod, currentMACDValue, _slowEMAPeriod, _fastEMAPeriod) { ParentOrdinal = this.Ordinal, Ordinal = childOrdinalStart + 2 };
         signalEMA.SetValue(chart);
      }
   }
}
