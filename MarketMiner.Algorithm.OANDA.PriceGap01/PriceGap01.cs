using MarketMiner.Algorithm.Common;
using MarketMiner.Algorithm.Common.Contracts;
using MarketMiner.Algorithm.OANDA.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Patterns.Dinapoli;
using MarketMiner.Api.Client.OANDA.Common;
using MarketMiner.Api.Client.OANDA.Data.DataModels;
using MarketMiner.Api.Common.Contracts;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.Extensions;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes;
using MarketMiner.Api.OANDA.TradeLibrary.DataTypes.Communications.Requests;
using MarketMiner.Client.Contracts;
using MarketMiner.Common.Enums;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using OANDAExt = MarketMiner.Api.OANDA.Extensions;
using MarketMiner.Common;

namespace MarketMiner.Algorithm.OANDA.Patterns
{
   [Export(typeof(IAlgorithm))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   [AlgorithmModule(Name = "PriceGap01", StrategyID = 3)]
   public class PriceGap01 : OANDAAlgorithmBase
   {
            [ImportingConstructor]
      public PriceGap01(ILogger logger)
         : base(logger)
      {
         _refreshInterval = 15000;
      }

      #region Declarations
      ThrustDetector _thrustDetector;      
      int _refreshInterval;
      #endregion

      private List<Tuple<string, EGranularity>> GetChartsSpecList()
      {
         var chartsSpecList = new List<Tuple<string, EGranularity>>() 
         { 
            Tuple.Create<string, EGranularity>(OANDAExt.Constants.Instruments.AUDUSD, EGranularity.M15),
            Tuple.Create<string, EGranularity>(OANDAExt.Constants.Instruments.NZDUSD, EGranularity.M15)
         };

         return chartsSpecList;
      }

      public override async Task<bool> Start()
      {
         await base.Initialize(2);
         AddAlgorithmMessage(string.Format("{0} is initialized.", _shortClassName));

         await base.Start();
         AddAlgorithmMessage(string.Format("{0} is started.", _shortClassName));

         Credentials.SetCredentials(EEnvironment.Practice, Security.Token.PracticeToken, Security.Account.PracticeAccount);

         try
         {
            while (_instance.Status == (short?)AlgorithmStatus.Starting)
            {
               if (!await OANDAExt.Helpers.IsMarketHalted())
               {
                  UpdateAlgorithmInstanceProperty("Status", (short?)AlgorithmStatus.RunningNoOpenTrades);

                  // build charts
                  _charts = new Dictionary<string, Chart>();
                  foreach (var spec in GetChartsSpecList())
                  {
                     string instrument = spec.Item1;
                     string granularity = spec.Item2.ToString();
                     _charts.Add(instrument + ":" + granularity, new Chart() { Instrument = instrument, Granularity = granularity });
                  }
                  
                  await InitializeCharts();

                  if (_chartsInitialized)
                  {
                     // start rates and events streams
                     //await StartRatesStreamAsync(OnRateReceived);
                     //base.Strategy.WriteStatusMessage("OANDA rates stream has started.");

                     //await StartEventsStreamAsync(OnEventReceived);
                     //base.Strategy.WriteStatusMessage("OANDA events stream has started.");

                     //await RunTradesTest();

                     await Run(_refreshInterval);
                  }
               }
               else
               {
                  AddAlgorithmMessage(string.Format("Market halted. {0}.Run() will be re-attempted in 30 minutes ...", _shortClassName));
                  await Task.Delay(TimeSpan.FromMinutes(30));
               }
            }
         }
         catch (Exception e)
         {
            AddAlgorithmMessage(e.Message).Wait();
         }

         return true;
      }

      public async Task InitializeCharts()
      {
         try
         {
            bool okToInitializeCharts = false;

            List<PriceBar> bars = null;

            while (!okToInitializeCharts)
            {
               bars = await RatesDataSource.Instance.GetPriceBars(GetChartsSpecList(), 100);

               if (bars.Count % 100 == 0)
               {
                  okToInitializeCharts = true;

                  foreach (var bar in bars)
                  {                    
                     string chartKey = bar.instrument + ":" + bar.granularity;
                     Chart chart = _charts[chartKey];
                     chart.Initialize();
                     chart.Frames.Add(CreateChartFrame(bar, chart));

                     //_writeMessage("\nBar spec is : " + chartKey);
                     //_writeMessage("Frame count is: " + chart.Frames.Count);
                  }

                  _chartsInitialized = true;
               }
               else
               {
                  await Task.Delay(TimeSpan.FromMinutes(5));
               }
            }

            AddAlgorithmMessage(string.Format("{0} charts ({1}) are initialized ...", _shortClassName, _charts.Count));
         }
         catch (Exception ex)
         {
            AddAlgorithmMessage(ex.Message).Wait();
         }
      }

      protected virtual async Task Run(int interval = 30000)
      {
         AddAlgorithmMessage(string.Format("{0} is running.", _shortClassName));

         List<PriceBar> bars = null;

         while (!_suspended())
         {
            bars = await RatesDataSource.Instance.GetPriceBars(GetChartsSpecList(), 1);

            bars.ForEach(b => AddAlgorithmMessage(string.Format("Current {0}: High: {1}, Low: {2}, Last: {3}", b.instrument, b.highBid, b.lowBid, b.closeBid)));

            UpdateCharts(bars);

            //ProcessCharts();

            await Task.Delay(TimeSpan.FromMilliseconds(interval));
         }
      }

      protected virtual void ProcessCharts()
      {
         if (_thrustDetector == null) _thrustDetector = new ThrustDetector();

         foreach (Chart chart in _charts.Values)
         {
            Thrust thrust = _thrustDetector.DetectThrust(chart);
            if (thrust != null) HandleThrustDetected(thrust);
         }
      }

      protected virtual void HandleThrustDetected(Thrust thrust)
      {
         if (thrust.SignalID == 0)
         {
            var signal = new MarketMiner.Client.Entities.Signal();
            signal.InjectFrom(thrust);

            WithClient(_serviceFactory.CreateClient<ISubscriptionService>(), subscriptionClient =>
            {
               signal = subscriptionClient.UpdateSignal(signal);
            });

            CreateFibRetracement();
            CreateEntryOrder();
            SendEntryOrder();
            SendThrustDetectedSignal(thrust);
         }
         else
         {
            ManageOrder(thrust);
         }
      }

      protected virtual void ManageOrder(Thrust thrust)
      {
         // api get the order status
         // if unfilled then update the entry/stop/limit as needed based on the refreshed post-Focus price action
         // if filled then update the stop/limit as needed based on the refreshed post-Focus price action

         bool filled = false;
         bool cancelled = false;

         while (!(filled || cancelled))
         {
            // manage order
         }
      }

      protected void SendThrustDetectedSignal(Thrust thrust)
      {
         thrust.SendPostmark = DateTime.UtcNow;
      }

      protected void CreateFibRetracement() { }
      protected void CreateEntryOrder()
      {
         
      }
      protected void SendEntryOrder() { }

      /// <summary>
      /// Updates the last bar in each chart for which a price bar is provided.
      /// The list passed in should only contain the most recent single bar for each instrument/granularity.
      /// </summary>
      /// <param name="bars"></param>
      private void UpdateCharts(List<PriceBar> bars)
      {
         foreach (PriceBar bar in bars)
         {
            // update or add frame from candle
            string chartKey = bar.instrument + ':' + bar.granularity;
            Chart chart = _charts[chartKey];

            var lastFrame = chart.Frames.Last();

            if (lastFrame.Bar.time == bar.time)
            {
               lastFrame.Bar = bar;

               chart.SetIndicatorValues(lastFrame);
            }
            else
            {
               chart.Frames.Add(CreateChartFrame(bar, chart));

               // keep the chart at 200 frames max size
               if (chart.Frames.Count > 200) chart.Frames.RemoveAt(0);
            }
         }
      }

      private Frame CreateChartFrame(IPriceBar bar, Chart chart)
      {
         Frame frame = chart.CreateStandardFrame(bar);

         int indicatorCount = frame.Indicators.Count;

         var list = new List<IIndicator>() 
         { 
            new DisplacedMovingAverage(3, 3) { Ordinal = indicatorCount + 1 }, 
            new DetrendedOscillator() { Ordinal = indicatorCount + 2 }, 
            new MACD(8, 17, 9) { Ordinal = indicatorCount + 3 } 
         };

         frame.AddIndicators(list);

         return frame;
      }

      #region Streaming data handlers
      private void OnRateReceived(RateStreamResponse data)
      {
         if (!data.IsHeartbeat())
         {
            // what should i do with this?
            AddAlgorithmMessage(string.Format("" + data.tick.instrument + " Time: " + data.tick.time + ", Bid: " + data.tick.bid + ", Ask: " + data.tick.ask));
         }
         else
         {
            AddAlgorithmMessage("rate heartbeat ---------");
         }

         Utilities.ReleaseTick();
      }

      protected void OnEventReceived(Event data)
      {
         if (!data.IsHeartbeat())
         {
            // what should i do with this?
            AddAlgorithmMessage(string.Format("Account: " + data.transaction.accountId + " Time: " + data.transaction.time + ", Type: " + data.transaction.type));
         }
         else
         {
            AddAlgorithmMessage("event heartbeat --------");
         }

         Utilities.ReleaseEvent();
      }
      #endregion
   }
}
