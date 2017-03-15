using System;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class FibonacciRetracement : Study
   {
      public FibonacciRetracement()
         : base(StudyType.FibonacciRetracement)
      {
         Ordinal = 0; // parent should always be the chart
      }

      double _previousFocusPrice, _currentFocusPrice;
      double? _previousExtremaPrice, _currentExtremaPrice;
      
      #region Properties
      /// <summary>The IPriceBar.lowMid value for the anchor bar.</summary>
      public double AnchorPrice { get; set; }
      /// <summary>The IPriceBar.time value for the anchor bar.</summary>
      public string AnchorTime { get; set; }
      /// <summary>The IPriceBar.time value for the focus bar.</summary>
      public string FocusTime { get; set; }
      /// <summary>The chart index of the extrema IPriceBar.</summary>
      public int? ExtremaIndex { get; set; }

      public double FocusPrice 
      { 
         get { return _currentFocusPrice; }
         set { _previousFocusPrice = _currentFocusPrice; _currentFocusPrice = value; if (FocusChanged()) OnPropertyChanged(); }
      }

      public double? ExtremaPrice
      {
         get { return _currentExtremaPrice; }
         set { _previousExtremaPrice = _currentExtremaPrice; _currentExtremaPrice = value; if (ExtremaChanged()) OnPropertyChanged(); }
      }
      #endregion

      #region level helpers
      public double LevelPrice(FibonacciLevel level)
      {
         double levelPrice = 0;

         int multiplier = FocusPrice > AnchorPrice ? -1 : 1;

         switch (level)
         {
            case FibonacciLevel.R382:
               levelPrice = FocusPrice + (Math.Abs(AnchorPrice - FocusPrice) * 0.382 * multiplier);
               break;
            case FibonacciLevel.R500:
               levelPrice = FocusPrice + (Math.Abs(AnchorPrice - FocusPrice) * 0.500 * multiplier);
               break;
            case FibonacciLevel.R618:
               levelPrice = FocusPrice + (Math.Abs(AnchorPrice - FocusPrice) * 0.618 * multiplier);
               break;
            default:
               throw new Exception(string.Format("The fibonacci level parameter provided [{0}] is not supported.", level.ToString()));
         }

         return levelPrice;
      }

      public int LevelPlaces()
      {
         // determine pip increment from # of decimal places
         // use the Max places of Anchor or Focus price in case of dropped 0s
         int places = 0;
         places = AnchorPrice.ToString().Split('.')[1].Length;
         places = Math.Max(places, FocusPrice.ToString().Split('.')[1].Length);

         // subtracting 1 until i understand how Mid prices has affected this
         places = places - 1;

         return places;
      }

      //public double LevelSpread(string side, string type)
      //{
      //   double divisor = Math.Pow(10, LevelPlaces() - 1);

      //   double levelSpread = 0;

      //   if (type == "entry")
      //      levelSpread = side == "buy" ? 4 / divisor : 2 / divisor;
      //   else if (type == "stopLoss")
      //      levelSpread = side == "buy" ? 2 / divisor : 4 / divisor;
      //   else
      //      throw new ArgumentException(string.Format("Level spread type {0} is invalid", type));

      //   return levelSpread;
      //}
      #endregion

      #region utilities
      public bool FocusChanged() { return _previousFocusPrice != _currentFocusPrice; }
      public bool ExtremaChanged() { return (_previousExtremaPrice ?? 0) != (_currentExtremaPrice ?? 0); }
      #endregion
   }

   public enum FibonacciLevel
   {
      R382,
      R500,
      R618
   };
}
