using MarketMiner.Api.Client.Common.Charting;
using P.Core.Common.Core;
using System;

namespace MarketMiner.Api.Client.Common.Patterns.Dinapoli
{
   public class Thrust : Pattern
   {
      public Thrust()
      {
         Type = MarketMiner.Api.Client.Common.Constants.SignalType.Thrust;
      }

      #region Declarations
      double _previousFocusPrice;
      double _currentFocusPrice;
      double _tickPrice;
      string _focusTime;
      bool _breakout;
      int? _fillZoneReachedIndex;
      int? _takeProfitZoneReachedIndex;
      double? _takeProfitZoneReachedFocusPrice;
      #endregion

      #region Properties
      public double? EntryPrice { get; private set; }
      public double? TakeProfitPrice { get; private set; }
      public double? StopLossPrice { get; private set; }
      public DateTime? EntryPriceUpdatedTime { get; private set; }
      public DateTime? TakeProfitPriceUpdatedTime { get; private set; }
      public DateTime? StopLossPriceUpdatedTime { get; private set; }
      public bool ProfitWindowClosed { get; set; }
      public int ReactionToFocusSpan { get; set; }
      public double TickPrice { get { return _tickPrice; } set { if (_tickPrice != value) { _tickPrice = value; OnPropertyChanged(); } } }
      public string FocusTime { get { return _focusTime; } set { if (_focusTime != value) { _focusTime = value; OnPropertyChanged(); } } }
      public bool Breakout { get { return _breakout; } set { if (_breakout != value) { _breakout = value; OnPropertyChanged(); } } }
      public double PreviousFocusPrice { get { return _previousFocusPrice; } }

      public double FocusPrice
      {
         get { return _currentFocusPrice; }
         set
         {
            _previousFocusPrice = _currentFocusPrice; _currentFocusPrice = value;
            if (FocusChanged()) CreateOrUpdateFibRetracement(); OnPropertyChanged();
         }
      }

      public int? FillZoneReachedIndex
      {
         get { return _fillZoneReachedIndex; }
         set { if (_fillZoneReachedIndex != value) { _fillZoneReachedIndex = value; OnPropertyChanged(); } }
      }

      public int? TakeProfitZoneReachedIndex
      {
         get { return _takeProfitZoneReachedIndex; }
         set { if (_takeProfitZoneReachedIndex != value) { _takeProfitZoneReachedIndex = value; OnPropertyChanged(); } }
      }

      public double? TakeProfitZoneReachedFocusPrice
      {
         get { return _takeProfitZoneReachedFocusPrice; }
         set { if (_takeProfitZoneReachedFocusPrice != value) { _takeProfitZoneReachedFocusPrice = value; OnPropertyChanged(); } }
      }
      #endregion

      public bool FocusChanged() { return _previousFocusPrice != _currentFocusPrice; }
      public bool FillZoneReached() { return _fillZoneReachedIndex.HasValue; }
      public bool TakeProfitZoneReached() { return _takeProfitZoneReachedIndex.HasValue; }

      #region Utilities
      protected void CreateOrUpdateFibRetracement()
      {
         FibonacciRetracement retracement = this.Study as FibonacciRetracement;

         if (retracement == null)
         {
            this.Study = new FibonacciRetracement() { AnchorPrice = SignalPrice, AnchorTime = SignalTime };
            retracement = this.Study as FibonacciRetracement;
         }

         retracement.FocusPrice = FocusPrice;
         retracement.FocusTime = FocusTime;
      }

      public void SetOrUpdatePrices(double? entryPrice, double? takeProfitPrice, double? stopLossPrice, int? places, DateTime? priceTime = null)
      {
         if (entryPrice.GetValueOrDefault() > 0)
         {
            this.EntryPrice = places == null ? entryPrice.Value : Math.Round(entryPrice.Value, places.Value);
            this.EntryPriceUpdatedTime = priceTime;
         }        
         if (takeProfitPrice.GetValueOrDefault() > 0)
         {
            this.TakeProfitPrice = places == null ? takeProfitPrice.Value : Math.Round(takeProfitPrice.Value, places.Value);
            this.TakeProfitPriceUpdatedTime = priceTime;
         }
         if (stopLossPrice.GetValueOrDefault() > 0)
         {
            this.StopLossPrice = places == null ? stopLossPrice.Value : Math.Round(stopLossPrice.Value, places.Value);
            this.StopLossPriceUpdatedTime = priceTime;
         }         
      }

      public void SetOrUpdatePriceUpdatedTimes(DateTime? entryPriceTime, DateTime? takeProfitPriceTime, DateTime? stopLossPriceTime)
      {
         if (entryPriceTime.HasValue)
            this.EntryPriceUpdatedTime = entryPriceTime;
         if (takeProfitPriceTime.HasValue)
            this.TakeProfitPriceUpdatedTime = takeProfitPriceTime;
         if (stopLossPriceTime.HasValue)
            this.StopLossPriceUpdatedTime = stopLossPriceTime;
      }
      #endregion
   }
}
