
using MarketMiner.Api.Common.Contracts;
using System;
namespace MarketMiner.Api.Client.Common.Charting
{
   public abstract class MovingAverage : Indicator
   {
      public MovingAverage()
      {
         Type = IndicatorType.SimpleMovingAverage;
         Period = 1;
      }

      public MovingAverage(IndicatorType type, short period)
      {
         Type = type;
         Period = period;
      } 
   }
}
