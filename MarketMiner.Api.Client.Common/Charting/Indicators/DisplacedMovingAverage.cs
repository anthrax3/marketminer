using System;
using System.Collections.Generic;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class DisplacedMovingAverage : MovingAverage
   {
      private const IndicatorType INDICATORTYPE = IndicatorType.DisplacedMovingAverage;

      #region Constructors
      public DisplacedMovingAverage(short period, short displacementX)
         : base(INDICATORTYPE, period)
      {
         DisplacementX = displacementX;
         DisplacementY = 0;
      }

      public DisplacedMovingAverage(short period, short displacementX, short displacementY)
         : base(INDICATORTYPE, period)
      {
         DisplacementX = displacementX;
         DisplacementY = displacementY;
      }
      #endregion

      public short DisplacementX { get; set; }
      public short DisplacementY { get; set; }

      public override List<IIndicator> SetValue(IChart chart, int childOrdinalStart)
      {
         Value = null;

         int frameCount = chart.Frames.Count;
         double closeMidTotal = 0;

         if (frameCount > Period + DisplacementX && DisplacementX == 3)
         {
            closeMidTotal += chart.Frames[frameCount - 4].Bar.closeMid;
            closeMidTotal += chart.Frames[frameCount - 5].Bar.closeMid;
            closeMidTotal += chart.Frames[frameCount - 6].Bar.closeMid;

            Value = closeMidTotal / 3;
         }
         else if (frameCount > Period + DisplacementX && DisplacementX == 7)
         {
            closeMidTotal += chart.Frames[frameCount - 8].Bar.closeMid;
            closeMidTotal += chart.Frames[frameCount - 9].Bar.closeMid;
            closeMidTotal += chart.Frames[frameCount - 10].Bar.closeMid;
            closeMidTotal += chart.Frames[frameCount - 11].Bar.closeMid;
            closeMidTotal += chart.Frames[frameCount - 12].Bar.closeMid;
            closeMidTotal += chart.Frames[frameCount - 13].Bar.closeMid;
            closeMidTotal += chart.Frames[frameCount - 14].Bar.closeMid;

            Value = closeMidTotal / 7;
         }

         return null;
      }
   }
}
