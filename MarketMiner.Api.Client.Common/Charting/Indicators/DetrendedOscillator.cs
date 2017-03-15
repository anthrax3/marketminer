using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class DetrendedOscillator : Indicator
   {
      private const IndicatorType INDICATORTYPE = IndicatorType.DetrendedOscillator;

      public DetrendedOscillator(short period = 7)
         : base(INDICATORTYPE, period)
      {
      }

      public override List<IIndicator> SetValue(IChart chart, int childOrdinalStart)
      {
         Value = null;

         int frameCount = chart.Frames.Count;
         int frameTotal = Period;
         double closeMidTotal = 0;
         double smaValue = 0;

         if (frameCount >= Period)
         {
            for (int i = frameCount - frameTotal; i < frameCount; i++)
            {
               closeMidTotal += chart.Frames[i].Bar.closeMid;
            }

            smaValue = closeMidTotal / Period;
            Value = chart.Frames[frameCount - 1].Bar.closeMid - smaValue;
         }

         return null;
      }
   }
}
