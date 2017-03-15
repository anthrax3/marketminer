
using MarketMiner.Api.Common.Contracts;
using System;
using System.Collections.Generic;
namespace MarketMiner.Api.Client.Common.Charting
{
   public abstract class Indicator : IIndicator
   {
      public Indicator() { }

      public Indicator(IndicatorType type)
      {
         Type = type;
      }

      public Indicator(IndicatorType type, short period)
      {
         Type = type;
         Period = period;
      }

      public double GetValue(int decimals = 5)
      {
         return Math.Round(Value.GetValueOrDefault(), decimals);
      }

      #region Interface.IIndicator
      public int Ordinal { get; set; }
      public int ParentOrdinal { get; set; }
      public short Period { get; set; }
      public IndicatorType Type { get; set; }
      public double? Value { get; set; }
      public virtual List<IIndicator> SetValue(IChart chart) { return SetValue(chart, -1); }
      public abstract List<IIndicator> SetValue(IChart chart, int childOrdinalStart);
      #endregion 
   }
}
