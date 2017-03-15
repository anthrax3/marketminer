using MarketMiner.Algorithm.Common;
using MarketMiner.Algorithm.OANDA.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Patterns.Dinapoli;
using MarketMiner.Api.Common.Contracts;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.Extensions;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes.Communications.Requests;
using MarketMiner.Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OANDAExt = MarketMiner.Api.OANDA.Extensions;

namespace MarketMiner.Algorithm.OANDA.Patterns
{
   //[AlgorithmModule(Name = "BreadAndButterAlgorithm")]
   //public class BreadAndButterAlgorithm : OANDAAlgorithmBase
   //{
      //   Thrust _thrust;
      //   ThrustDetector _detector;
      //   Func<bool> _thrustDetected;
      //   Candle _candleZero;
      //   double _thresholdSpread;
      //   bool _executed;
      //   Dictionary<string, Chart> _charts;

      //   public BreadAndButterAlgorithm(int accountId, string instrument, short granularity, double thresholdSpread, double thresholdGap)
      //      : base(new Strategy() { StrategyID = 1, Granularity = granularity, Instruments = new List<string>() { instrument } })
      //   {
      //      _accountId = accountId;
      //   }

      //   public override async Task<bool> Start()
      //   {
      //      await base.Start();

      //      try
      //      {
      //         while (Strategy.AlgorithmStatus == (short)EAlgorithmStatus.Starting)
      //         {
      //            if (!await OANDAExt.Helpers.IsMarketHalted())
      //            {
      //               Strategy.AlgorithmStatus = (short)EAlgorithmStatus.RunningNoOpenTrades;

      //               _charts = new Dictionary<string, Chart>();
      //               _charts.Add("USD_JPY:M15", new Chart() { Instrument = "USD_JPY", Granularity = "M15" });
      //               _charts.Add("USD_JPY:H1", new Chart() { Instrument = "USD_JPY", Granularity = "H1 " });

      //               await InitializeCharts();
      //               //StartRatesStream(OnRateReceived);
      //               //StartEventsStream(OnEventReceived);
      //            }
      //            else
      //            {
      //               await Task.Delay(TimeSpan.FromMinutes(30));
      //            }
      //         }
      //      }
      //      catch (Exception ex)
      //      {
      //         base.Strategy.WriteStatusMessage(ex.Message);
      //      }

      //      return true;
      //   }

      //   public async Task InitializeCharts()
      //   {
      //      try
      //      {
      //         bool okToInitializeCharts = false;

      //         Func<CandlesRequest> requestM15 = () => new CandlesRequest { instrument = OANDAExt.Constants.Instruments.USDJPY, granularity = EGranularity.M15 };
      //         CandlesRequest initialRequestM15 = null;
      //         List<Candle> initialCandlesM15 = null;

      //         Func<CandlesRequest> requestH1 = () => new CandlesRequest { instrument = OANDAExt.Constants.Instruments.USDJPY, granularity = EGranularity.H1 };
      //         CandlesRequest initialRequestH1 = null;
      //         List<Candle> initialCandlesH1 = null;

      //         while (!okToInitializeCharts)
      //         {
      //            initialRequestM15 = requestM15();
      //            initialRequestM15.count = 100;
      //            initialCandlesM15 = (await Rest.GetCandlesAsync(initialRequestM15));

      //            initialRequestH1 = requestH1();
      //            initialRequestH1.count = 100;
      //            initialCandlesH1 = (await Rest.GetCandlesAsync(initialRequestH1));

      //            if (initialCandlesM15.Count == 100 && initialCandlesH1.Count == 100)
      //            {
      //               okToInitializeCharts = true;

      //               UpdateCharts(initialCandlesM15.Cast<ICandle>().ToList(), "USD_JPY:M15");
      //               UpdateCharts(initialCandlesH1.Cast<ICandle>().ToList(), "USD_JPY:H1");
      //            }
      //            else
      //            {
      //               await Task.Delay(TimeSpan.FromMinutes(5));
      //            }
      //         }
      //      }
      //      catch (Exception ex)
      //      {
      //         base.Strategy.WriteStatusMessage(ex.Message);
      //      }
      //   }

      //   private void OnRateReceived(RateStreamResponse data)
      //   {
      //      if (!data.IsHeartbeat())
      //      {
      //         // what should i do with this?
      //      }

      //      Utilities.ReleaseTick();
      //   }

      //   protected void OnEventReceived(Event data)
      //   {
      //      if (data.transaction != null)
      //      {
      //         // what should i do with this?
      //      }
      //      Utilities.ReleaseEvent();
      //   }

      //   protected void ScanForThrust()
      //   {
      //      Thrust thrust = null;

      //      // if there are 10 (this should be a configuration item) frames with 3x3 values then proceed
      //      foreach (Chart chart in _charts.Values)
      //      {
      //         if (chart.Frames.Count >= 10)
      //         {
      //            Frame frame;
      //            short above3x3Count = 0, below3x3Count = 0;

      //            for (int i = chart.Frames.Count - 1; i >= 0; i--)
      //            {
      //               frame = chart.Frames[i];

      //               ICandle candle = frame.Candle;
      //               IIndicator displaced3x3 = frame.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage);

      //               var candleOpenBid = Math.Round(candle.openBid, 2);
      //               var candleCloseBid = Math.Round(candle.closeBid, 2);
      //               var candleHighBid = Math.Round(candle.highBid, 2);
      //               var candleLowBid = Math.Round(candle.lowBid, 2);

      //               // above or below the 3x3
      //               if (candle.highBid >= displaced3x3.Value) below3x3Count++;
      //               if (candle.lowBid >= displaced3x3.Value) above3x3Count++;

      //               if (above3x3Count == 7 && below3x3Count == 0)
      //               {
      //                  thrust = BuildThrust(EThrustDirection.Up);
      //                  HandleThrustDetected(thrust);
      //               }

      //               if (above3x3Count == 7 && below3x3Count == 0)
      //               {
      //                  thrust = BuildThrust(EThrustDirection.Down);
      //                  HandleThrustDetected(thrust);
      //               }
      //            }
      //         }
      //         else
      //         {
      //            base.Strategy.WriteStatusMessage("Thrust tests did not receive candles.");
      //         }
      //      }
      //   }

      //   private Thrust BuildThrust(EThrustDirection direction)
      //   {
      //      return new Thrust() { Direction = direction };
      //   }

      //   protected virtual async void HandleThrustDetected(Thrust thrust)
      //   {
      //      SendThrustDetectedSignal();
      //      CreateFibRetracement();
      //      CreateEntryOrder();
      //      SendEntryOrder();
      //      ManageOrder();
      //   }

      //   protected virtual void ManageOrder()
      //   {
      //      bool filled = false;
      //      bool cancelled = false;

      //      while (!(filled || cancelled))
      //      {
      //         // manage order
      //      }
      //   }

      //   protected void SendThrustDetectedSignal() { }
      //   protected void CreateFibRetracement() { }
      //   protected void CreateEntryOrder() { }
      //   protected void SendEntryOrder() { }

      //   private void UpdateCharts(List<ICandle> candles, string chartKey)
      //   {
      //      foreach (var chart in _charts.Values) chart.Initialize();

      //      // create frames from candles
      //      foreach (ICandle candle in candles)
      //      {
      //         var frame = new Frame() { Candle = candle };
      //         var displaced3x3 = new DisplacedMovingAverage(3, 3);
      //         var list = new List<IIndicator>() { new SimpleMovingAverage(50), new SimpleMovingAverage(200), displaced3x3 };
      //         frame.Indicators = list;

      //         _charts[chartKey].Frames.Add(frame);
      //      }
      //   }

      //   private void UpdateCharts(List<ICandle> candles)
      //   {
      //      foreach (var chart in _charts.Values) chart.Initialize();

      //      // create frames from candles
      //      foreach (CandleMM candle in candles)
      //      {
      //         string chartKey = candle.instrument + ':' + candle.granularity;
      //         Chart candleChart = _charts[chartKey];
      //         ICandle lastChartCandle = candleChart.Frames.Last().Candle;

      //         // remove older candles
      //         if (candle.time != lastChartCandle.time && candle.volume > 1)
      //            candles.Remove(candle);

      //         // udpate last candle
      //         if (candle.time == lastChartCandle.time)
      //         {
      //            // update the candle
      //            lastChartCandle.openMid = candle.openMid;
      //            lastChartCandle.highMid = candle.highMid;
      //            lastChartCandle.lowMid = candle.lowMid;
      //            lastChartCandle.closeMid = candle.closeMid;
      //            lastChartCandle.openBid = candle.openBid;
      //            lastChartCandle.highBid = candle.highBid;
      //            lastChartCandle.lowBid = candle.lowBid;
      //            lastChartCandle.closeBid = candle.closeBid;
      //            lastChartCandle.openAsk = candle.openAsk;
      //            lastChartCandle.highAsk = candle.highAsk;
      //            lastChartCandle.lowAsk = candle.lowAsk;
      //            lastChartCandle.closeAsk = candle.closeAsk;
      //            lastChartCandle.volume = candle.volume;
      //         }

      //         // add a new candle
      //         if (candle.volume == 1)
      //         {
      //            lastChartCandle.complete = true;

      //            var frame = new Frame() { Candle = candle };
      //            var displaced3x3 = new DisplacedMovingAverage(3, 3);
      //            var list = new List<IIndicator>() { new SimpleMovingAverage(50), new SimpleMovingAverage(200), displaced3x3 };
      //            frame.Indicators = list;

      //            candleChart.Frames.Add(frame);
      //         }

      //         // keep the chart a 200 frames max size
      //         if (candleChart.Frames.Count > 200) candleChart.Frames.RemoveAt(0);
      //      }
      //   }

      //   private void UpdateCharts(Price tick)
      //   {
      //      UpdateCharts(GetCompleteBars(tick));
      //   }

      //   private List<ICandle> GetCompleteBars(Price tick = null)
      //   {
      //      string instrument = tick.instrument;
      //      string time = tick != null ? tick.time : null;
      //      DateTime tickTime = new DateTime();
      //      DateTime.TryParse(time, out tickTime);
      //      double midPrice = tick != null ? (tick.bid + tick.ask) / 2 : 0;
      //      List<ICandle> completeBars;

      //      InitBars(instrument);
      //      completeBars = FilterBars(instrument, tickTime);
      //      UpdateBars(instrument, tickTime, midPrice);

      //      return completeBars;
      //   }

      //   private Dictionary<string, Dictionary<string, CandleMM>> _currentBars = new Dictionary<string, Dictionary<string, CandleMM>>();

      //   private void InitBars(string instrument)
      //   {
      //      if (!_currentBars.Keys.Contains(instrument))
      //      {
      //         _currentBars.Add(instrument, new Dictionary<string, CandleMM>());

      //         foreach (Tuple<string, long> coeff in OANDAExt.Constants.Coefficients)
      //         {
      //            _currentBars[instrument].Add(coeff.Item1, null);
      //         }
      //      }
      //   }

      //   private List<ICandle> FilterBars(string instrument, DateTime tickTime)
      //   {
      //      List<ICandle> completeBars = new List<ICandle>();

      //      foreach (Tuple<string, long> coeff in OANDAExt.Constants.Coefficients)
      //      {
      //         CandleMM bar = _currentBars[instrument][coeff.Item1];
      //         double nowTime = Math.Floor(tickTime.ToFileTimeUtc() / coeff.Item2 * 1.00) * coeff.Item2;
      //         bool isNewTime = bar != null && bar.time != null && bar.time.ToUpper() != nowTime.ToString().ToUpper();

      //         if (isNewTime)
      //         {
      //            completeBars.Add(new CandleMM
      //            {
      //               instrument = instrument,
      //               granularity = coeff.Item1,
      //               time = bar.time,
      //               openMid = bar.open,
      //               highMid = bar.high,
      //               lowMid = bar.low,
      //               closeMid = bar.close,
      //               volume = bar.volume
      //            });
      //         }
      //      }

      //      return completeBars;
      //   }

      //   private void UpdateBars(string instrument, DateTime tickTime, double midPrice)
      //   {
      //      foreach (Tuple<string, long> coeff in OANDAExt.Constants.Coefficients)
      //      {
      //         CandleMM bar = _currentBars[instrument][coeff.Item1] ?? new CandleMM();
      //         double timeBar = Math.Floor(tickTime.ToFileTimeUtc() / coeff.Item2 * 1.00) * coeff.Item2;
      //         bool isNewBar = bar.time == null;
      //         bool isNewTime = bar.time == null ? true : bar.time.ToUpper() != timeBar.ToString().ToUpper();

      //         if (isNewBar || isNewTime)
      //         {
      //            bar.instrument = instrument;
      //            bar.granularity = coeff.Item1;
      //            bar.open = midPrice;
      //            bar.close = midPrice;
      //            bar.high = midPrice;
      //            bar.low = midPrice;
      //            bar.volume = 0;
      //            bar.time = timeBar.ToString();
      //         }
      //         else if (midPrice > bar.high)
      //         {
      //            bar.high = midPrice;
      //         }
      //         else if (midPrice < bar.low)
      //         {
      //            bar.low = midPrice;
      //         }

      //         bar.close = midPrice;
      //         bar.volume += 1;
      //      }
      //   }
      //}
   //}

   class CandleMM : IPriceBar
   {
      public string time { get; set; }
      public int volume { get; set; }
      public bool complete { get; set; }
      public double openMid { get; set; }
      public double highMid { get; set; }
      public double lowMid { get; set; }
      public double closeMid { get; set; }
      public double openBid { get; set; }
      public double highBid { get; set; }
      public double lowBid { get; set; }
      public double closeBid { get; set; }
      public double openAsk { get; set; }
      public double highAsk { get; set; }
      public double lowAsk { get; set; }
      public double closeAsk { get; set; }
      //

      public string instrument { get; set; }
      public string granularity { get; set; }
      public double open { get; set; }
      public double high { get; set; }
      public double low { get; set; }
      public double close { get; set; }
   }
}

