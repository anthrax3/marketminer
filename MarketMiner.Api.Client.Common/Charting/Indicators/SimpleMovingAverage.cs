using System;
using System.Collections.Generic;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class SimpleMovingAverage : MovingAverage
   {
      private const IndicatorType INDICATORTYPE = IndicatorType.SimpleMovingAverage;

      public SimpleMovingAverage(short period)
         : base(INDICATORTYPE, period)
      {
      }

      public override List<IIndicator> SetValue(IChart chart, int childOrdinalStart)
      {
         Value = null;

         int frameCount = chart.Frames.Count;
         int frameTotal = Period;
         double closeMidTotal = 0;

         if (frameCount >= Period)
         {
            for (int i = frameCount - frameTotal; i < frameCount; i++)
            {
               closeMidTotal += chart.Frames[i].Bar.closeMid;
            }

            Value = closeMidTotal / Period;
         }

         return null;
      }
   }
}
