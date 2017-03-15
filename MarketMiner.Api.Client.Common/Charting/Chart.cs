using MarketMiner.Api.Client.Common.Patterns;
using MarketMiner.Api.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace MarketMiner.Api.Client.Common.Charting
{
   public class Chart : IChart
   {
      public Chart()
      {
      }

      #region Properties
      public string Instrument { get; set; }
      public string Granularity { get; set; }
      public double GranularitySeconds { get; set; }
      public ObservableCollection<Frame> Frames { get; set; }
      public List<Pattern> Patterns { get; private set; }
      public List<IStudy> Studies { get; private set; }
      public bool LastFrameUpdated { get; set; }
      public double HistoricBidAskSpread { get; set; }

      public DateTime? CreateNewFrameTime
      {
         get
         {
            return Convert.ToDateTime(Frames.Last().Bar.time).ToUniversalTime().AddSeconds(GranularitySeconds);
         }
      }
      #endregion

      public void Initialize()
      {
         Frames = new ObservableCollection<Frame>();
         Frames.CollectionChanged += OnFramesCollectionChanged;

         Patterns = new List<Pattern>();
         Studies = new List<IStudy>();
      }

      public void SetIndicatorValues(Frame frame)
      {
         // first, remove all child indicators
         frame.RemoveIndicators(frame.Indicators.Where(i => i.ParentOrdinal != 0));

         var indicators = frame.Indicators as IEnumerable<IIndicator>;
         var childIndicators = new List<IIndicator>();

         foreach (IIndicator indicator in indicators)
         {
            int childOrdinalStart = frame.Indicators.Count + 1 + childIndicators.Count;

            List<IIndicator> returnIndicators = indicator.SetValue(this, childOrdinalStart);

            // some indicators create child indicators
            if (returnIndicators != null) childIndicators.AddRange(returnIndicators);
         }

         // add the child indicators to the frame
         if (childIndicators.Count > 0) frame.AddIndicators(childIndicators);
      }

      public Frame CreateStandardFrame(IPriceBar bar)
      {
         var frame = new Frame() { Bar = bar };
         var list = new List<IIndicator>() 
         { 
            new SimpleMovingAverage(50) { Ordinal = 1 }, 
            new SimpleMovingAverage(200) { Ordinal = 2 }, 
         };

         frame.AddIndicators(list);

         return frame;
      }

      public void AddIndicator(IIndicator indicator)
      {
         foreach (Frame frame in Frames)
         {
            frame.AddIndicator(indicator);
            SetIndicatorValues(frame);
         }
      }

      #region Private methods
      private void OnFramesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         switch (e.Action)
         {
            case NotifyCollectionChangedAction.Add:
               foreach (var item in e.NewItems)
               {
                  var frame = item as Frame;
                  SetIndicatorValues(frame);
               }
               break;
            case NotifyCollectionChangedAction.Replace:
               break;
            case NotifyCollectionChangedAction.Move:
               break;
            case NotifyCollectionChangedAction.Remove:
               break;
            case NotifyCollectionChangedAction.Reset:
               break;
         }
      }
      #endregion
   }

   public interface IChart
   {
      void Initialize();
      ObservableCollection<Frame> Frames { get; set; }
      string Instrument { get; set; }
      string Granularity { get; set; }
      void AddIndicator(IIndicator indicator);
      Frame CreateStandardFrame(IPriceBar bar);
   }
}
