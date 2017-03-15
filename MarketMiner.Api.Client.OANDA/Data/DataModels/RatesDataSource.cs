using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.OANDA.ViewModels;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications.Requests;
using P.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMiner.Api.Client.OANDA.Data.DataModels
{
   public class RatesDataSource
   {
      private static RatesDataSource _ratesDataSource = new RatesDataSource();

      private ObservableCollection<CandleData> _allCandles = new ObservableCollection<CandleData>();

      public static RatesDataSource Instance { get { return new RatesDataSource(); } }

      public ObservableCollection<CandleData> AllCandles
      {
         get { return _allCandles; }
      }

      public static CandleData GetCandles(string instrumentName, string granularity)
      {
         var matches = _ratesDataSource._allCandles.Where((group) => group.UniqueId.Equals(instrumentName + granularity));
         if (matches.Count() == 1) return matches.First();
         // request the missing data
         var newGroup = new CandleData(instrumentName, granularity);
         _ratesDataSource._allCandles.Add(newGroup);
         return newGroup;
      }

      public async Task<List<PriceBar>> GetPriceBars(List<Tuple<string, EGranularity>> barSpecsList, int count, ECandleFormat priceFormat = ECandleFormat.midpoint)
      {
         List<PriceBar> bars = new List<PriceBar>();

         foreach (var spec in barSpecsList)
         {
            Func<CandlesRequest> request = () => new CandlesRequest
            {
               instrument = spec.Item1,
               granularity = spec.Item2,
               candleFormat = priceFormat,
               count = count
            };

            List<Candle> pollCandles = null;

            pollCandles = await Rest.GetCandlesAsync(request());

            if (pollCandles != null && pollCandles.Count > 0)
            {
               pollCandles.OrderBy(c => Convert.ToDateTime(c.time).ToUniversalTime());

               foreach (var candle in pollCandles)
               {
                  var bar = new PriceBar() { instrument = spec.Item1, granularity = spec.Item2.ToString() };
                  bar.InjectWith(candle, false);
                  bars.Add(bar);
               }
            }
         }

         return bars;
      }
   }

   public class CandleData : DataGroup
   {
      public string Instrument { get; set; }
      public string Granularity { get; set; }

      public CandleData(string instrument, string granularity)
         : base(instrument + granularity, instrument, granularity, "", "")
      {
         Instrument = instrument;
         Granularity = granularity;
         RequestDataUpdate();
      }

      public async void RequestDataUpdate()
      {
         var candles = await Rest.GetCandlesAsync(Instrument, Granularity);
         Items.Clear();
         foreach (var candle in candles)
         {
            Items.Add(new CandleViewModel(candle, this));
         }
      }
   }
}
