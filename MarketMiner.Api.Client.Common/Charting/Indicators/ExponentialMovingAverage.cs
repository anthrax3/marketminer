using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class ExponentialMovingAverage : MovingAverage
   {
      const IndicatorType INDICATORTYPE = IndicatorType.ExponentialMovingAverage;
      bool _isMACDSignalEMA;
      short _slowMACDPeriod, _fastMACDPeriod;
      double _currentMACDValue;

      public ExponentialMovingAverage(short period)
         : base(INDICATORTYPE, period)
      {
      }

      public ExponentialMovingAverage(short period, double currentMACDValue, short slowMACDPeriod, short fastMACDPeriod)
         : base(INDICATORTYPE, period)
      {
         _isMACDSignalEMA = true;
         _currentMACDValue = currentMACDValue;
         _slowMACDPeriod = slowMACDPeriod;
         _fastMACDPeriod = fastMACDPeriod;
      }

      public override List<IIndicator> SetValue(IChart chart, int childOrdinalStart)
      {
         Value = null;

         int frameCount = chart.Frames.Count;
         int frameTotal = Period;
         double closeMidTotal = 0;
         double smoothingFactor = 2 / ((double)1.0 + Period);

         if (_isMACDSignalEMA)
         {
            return CalcSignalEMAValue(chart);
         }

         if (chart.Frames.Count >= Period)
         {
            for (int i = frameCount - frameTotal; i < frameCount; i++)
            {
               closeMidTotal += chart.Frames[i].Bar.closeMid;
            }

            if (chart.Frames.Count == Period)
            {
               Value = closeMidTotal / Period;
            }
            else
            {
               double thisFrameCandlecloseMid = chart.Frames[frameCount - 1].Bar.closeMid;

               double previousFrameCandleEMAValue = chart.Frames[frameCount - 2].Indicators.Where(i => i.Type == IndicatorType.ExponentialMovingAverage && i.ParentOrdinal == this.ParentOrdinal && i.Period == this.Period).First().Value ?? 0;

               Value = (smoothingFactor * (thisFrameCandlecloseMid - previousFrameCandleEMAValue)) + previousFrameCandleEMAValue;
            }
         }

         return null;
      }

      private List<IIndicator> CalcSignalEMAValue(IChart chart)
      {
         ExponentialMovingAverage slowEMA, fastEMA;
         int frameCount = chart.Frames.Count;
         int frameTotal = Period;
         double macdTotal = 0;
         double smoothingFactor = 2 / ((double)1.0 + Period);

         if (frameCount >= Period)
         {
            // start with the current macd value because the current slow & fast ema indicators
            // will not be in the frame yet .. thus the i < frameCount - 1 in the for(..)
            macdTotal = _currentMACDValue;

            for (int i = frameCount - frameTotal; i < frameCount - 1; i++)
            {
               slowEMA = chart.Frames[i].Indicators.Where(k => k.Type == IndicatorType.ExponentialMovingAverage && k.ParentOrdinal == this.ParentOrdinal && k.Period == _slowMACDPeriod).First() as ExponentialMovingAverage;
               fastEMA = chart.Frames[i].Indicators.Where(k => k.Type == IndicatorType.ExponentialMovingAverage && k.ParentOrdinal == this.ParentOrdinal && k.Period == _fastMACDPeriod).First() as ExponentialMovingAverage;

               macdTotal += (slowEMA.Value - fastEMA.Value) ?? 0;
            }

            if (frameCount == Period)
            {
               Value = macdTotal / Period;
            }
            else
            {
               double previousSlowEMAValue = chart.Frames[frameCount - 2].Indicators.Where(k => k.Type == IndicatorType.ExponentialMovingAverage && k.ParentOrdinal == this.ParentOrdinal && k.Period == _slowMACDPeriod).First().Value.GetValueOrDefault();
               double previousFastEMAValue = chart.Frames[frameCount - 2].Indicators.Where(k => k.Type == IndicatorType.ExponentialMovingAverage && k.ParentOrdinal == this.ParentOrdinal && k.Period == _fastMACDPeriod).First().Value.GetValueOrDefault();

               double previousMACDValue = previousSlowEMAValue - previousFastEMAValue;
               //

               Value = (smoothingFactor * (_currentMACDValue - previousMACDValue)) + previousMACDValue;
            }
         }

         return null;
      }
   }
}
