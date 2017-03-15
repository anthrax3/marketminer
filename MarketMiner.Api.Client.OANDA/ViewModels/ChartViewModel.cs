using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.OANDA.Common;
using MarketMiner.Api.Client.OANDA.Data.DataModels;
using System;
using System.Collections.ObjectModel;
using OANDAExt = MarketMiner.Api.OANDA.Extensions;

namespace MarketMiner.Api.Client.OANDA.ViewModels
{
   public class ChartViewModel : BindableBase, IChart
   {
      public string Instrument { get; set; }
      public string Period { get; set; }

      public ChartViewModel(String instrument)
      {
         Instrument = instrument;

         // Note: we currently don't support changing the period
         Period = OANDAExt.Constants.Granularity.Hour01;
      }

      public CandleData Candles
      {
         get { return RatesDataSource.GetCandles(Instrument, Period); }
      }

      public void Initialize()
      {
         throw new NotImplementedException();
      }

      public ObservableCollection<Frame> Frames { get; set; }

      public string Granularity { get; set; }


      public void AddIndicator(IIndicator indicator)
      {
         throw new NotImplementedException();
      }


      public Frame CreateStandardFrame(Api.Common.Contracts.IPriceBar bar)
      {
         throw new NotImplementedException();
      }
   }
}
