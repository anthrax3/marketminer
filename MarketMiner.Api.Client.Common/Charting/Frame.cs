using MarketMiner.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class Frame : IFrame
   {
      public Frame()
      {
         _indicators = new List<IIndicator>();
      }

      private List<IIndicator> _indicators;

      public IPriceBar Bar { get; set; }
      public ReadOnlyCollection<IIndicator> Indicators { get { return new ReadOnlyCollection<IIndicator>(_indicators); } }

      /// <summary>
      /// Add indicator to the frame
      /// </summary>
      /// <param name="indicators"></param>
      /// <returns>Number of indicators added</returns>
      public int AddIndicator(IIndicator indicator)
      {
         return AddIndicators(new[] { indicator });
      }

      /// <summary>
      /// Add indicators to the frame
      /// </summary>
      /// <param name="indicators"></param>
      /// <returns>Number of indicators added</returns>
      public int AddIndicators(IEnumerable<IIndicator> indicators)
      {
         indicators.OrderBy(i => i.Ordinal);

         int indicatorsAdded = 0;

         foreach (var indicator in indicators)
         {
            if (ValidateIndicatorOrdinal(indicator))
               _indicators.Add(indicator);

            indicatorsAdded++;
         }

         return indicatorsAdded;
      }

      private bool ValidateIndicatorOrdinal(IIndicator indicator)
      {
         if (indicator.Ordinal == Indicators.Count + 1)
            return true;
         else
         {
            throw new Exception("Indicator additions must have unique ordinals that are continguous from the current Indicators count.");
         }  
      }

      /// <summary>
      /// Remove indicator from the frame
      /// </summary>
      /// <param name="indicators"></param>
      /// <returns>Number of indicators removed</returns>
      public int RemoveIndicator(IIndicator indicator)
      {
         return RemoveIndicators(new []{ indicator });
      }

      /// <summary>
      /// Remove indicators from the frame
      /// </summary>
      /// <param name="indicators"></param>
      /// <returns>Number of indicators removed</returns>
      public int RemoveIndicators(IEnumerable<IIndicator> indicators)
      {
         int indicatorsRemoved;

         indicatorsRemoved = _indicators.RemoveAll(i => indicators.Contains(i));

         //foreach (var indicator in indicators)
         //{
         //   _indicators.Remove(indicator);
         //   indicatorsRemoved++;
         //}

         return indicatorsRemoved;
      }
   }

   public interface IFrame
   {
      IPriceBar Bar { get; set; }
      ReadOnlyCollection<IIndicator> Indicators { get; }
      int AddIndicator(IIndicator indicator);
      int AddIndicators(IEnumerable<IIndicator> indicators);
      int RemoveIndicator(IIndicator indicator);
      int RemoveIndicators(IEnumerable<IIndicator> indicators);
   }
}
