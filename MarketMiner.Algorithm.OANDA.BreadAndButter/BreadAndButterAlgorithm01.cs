using MarketMiner.Algorithm.Common;
using MarketMiner.Algorithm.Common.Contracts;
using MarketMiner.Algorithm.OANDA.Common;
using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.Common.Contracts;
using MarketMiner.Api.Client.Common.Patterns.Dinapoli;
using MarketMiner.Api.Client.OANDA.Data;
using MarketMiner.Api.Common.Contracts;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.Extensions.Classes;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications.Requests;
using MarketMiner.Client.Entities;
using MarketMiner.Client.Proxies.ServiceCallers;
using MarketMiner.Common.Enums;
using N.Core.Common.Contracts;
using N.Core.Common.Core;
using N.Core.Common.Exceptions;
using P.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MACC = MarketMiner.Api.Client.Common;
using MAOE = MarketMiner.Api.OANDA.Extensions;
using MCE = MarketMiner.Client.Entities;

namespace MarketMiner.Algorithm.OANDA.Patterns
{
   [Export(typeof(IAlgorithm))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   [AlgorithmModule(Name = "BreadAndButterAlgorithm01", StrategyID = 1)]
   public class BreadAndButterAlgorithm01 : OANDAAlgorithmBase
   {
      #region Declarations
      protected IPatternDetector _thrustDetector;
      protected IPatternFactory _thrustFactory;
      #endregion

      [ImportingConstructor]
      public BreadAndButterAlgorithm01(IEventLogger logger)
         : base(logger)
      {
      }

      #region Start & Run
      /// <summary>
      /// Creates and starts the algorithm instance.
      /// </summary>
      /// <returns>Returns true if an algorithm instance is successfully started. False if an algorithm instance is not successfully started.</returns>
      public override async Task<bool> Start()
      {
         bool result = true;

         try
         {
            await base.Start();

            while (_instance.Status == (short?)AlgorithmStatus.Starting)
            {
               if (DataManager.ApiConnectionStatus == ApiConnectionStatus.Recovering)
                  await Task.Delay(TimeSpan.FromMinutes(1));
               else if (DataManager.ApiConnectionStatus == ApiConnectionStatus.Disconnected
                  || (!MAOE.Utilities.EventsStreamStarted && DataManager.ApiConnectionStatus == ApiConnectionStatus.Connected))
               {
                  if (!await MAOE.Helpers.IsMarketHalted())
                  {
                     // build charts
                     _charts.Clear();
                     foreach (var spec in GetChartsSpecList())
                     {
                        string instrument = spec.Item1;
                        string granularity = spec.Item2.ToString();
                        Chart chart = new Chart()
                        {
                           Instrument = instrument,
                           Granularity = granularity,
                           GranularitySeconds = MAOE.Helpers.GranularityToSeconds(granularity)
                        };
                        _charts.Add(instrument + ":" + granularity, chart);
                     }

                     // hook up DataManager event listeners
                     DataManager.ConnectionStatusChanged += OnConnectionStatusChanged;
                     DataManager.ConnectionRecoveryMessaged += OnConnectionRecoveryMessaged;

                     // start events stream
                     await DataManager.StartEventsStreamAsync(null);

                     if (DataManager.ApiConnectionStatus == ApiConnectionStatus.Streaming && MAOE.Utilities.EventsStreamStarted)
                     {
                        AddAlgorithmMessage(DataManager.RegisterDataStreamHandler<Event>(OnEventReceived));

                        await InitializeCharts();

                        // start rates stream
                        //if (!MAOE.Utilities.RatesStreamStarted) 
                        //{
                        //   List<string> instruments = new List<string>();
                        //   GetChartsSpecList().ForEach(spec => instruments.Add(spec.Item1));
                        //   await DataManager.StartRatesStreamAsync(instruments, null);  
                        //}             
                        //else
                        //   AddAlgorithmMessage(DataManager.RegisterDataStreamHandler<RateStreamResponse>(OnRateReceived));

                        AddAlgorithmMessage(string.Format("{0} is started.", _shortClassName), true, TraceEventType.Start);

                        result = await Run();
                     }
                  }
                  else
                  {
                     AddAlgorithmMessage(string.Format("Market halted. {0}.Run() will be re-attempted in 30 minutes.", _shortClassName), false, TraceEventType.Information);
                     await Task.Delay(TimeSpan.FromMinutes(30));
                  }
               }
            }
         }
         catch (Exception e)
         {
            Exception ex = new Exception(string.Format("Algorithm for strategyId: {0} failed to start.", _strategy.StrategyID), e);
            AddAlgorithmException(ex).Wait();
            return false;
         }

         return result;
      }

      /// <summary>
      /// Runs the algorithm instance.
      /// </summary>
      /// <param name="refreshInterval"></param>
      /// <returns></returns>
      protected virtual async Task<bool> Run()
      {
         try
         {
            UpdateAlgorithmInstanceProperty("Status", (short?)AlgorithmStatus.Running);

            AddAlgorithmMessage(string.Format("{0} is running.", _shortClassName), true, TraceEventType.Information);

            List<PriceBar> bars = null;

            while (!_suspended())
            {
               if (_chartsInitialized
                  && (CoreUtilities.InList<short>((short)DataManager.ApiConnectionStatus
                  , (short)ApiConnectionStatus.Connected, (short)ApiConnectionStatus.Streaming)))
               {
                  List<Tuple<string, EGranularity>> specList = GetChartsSpecList();

                  bars = await DataManager.Instance.GetPriceBars(specList, 2);

                  if (bars != null)
                  {
                     if (bars.Count != (specList.Count * 2))
                     {
                        string message = string.Format("RatesDataSource did not return expected count {0} of bars.", specList.Count);
                        AddAlgorithmMessage(message, true, TraceEventType.Information);
                     }
                     else
                     {
                        bars.ForEach(b => AddAlgorithmMessage(string.Format("{0}: High: {1}, Low: {2}, Last: {3}", b.instrument, b.highMid, b.lowMid, b.closeMid), false));

                        UpdateCharts(bars);

                        ProcessCharts();
                     }
                  }
               }

               await Task.Delay(TimeSpan.FromMilliseconds(_chartRefreshMilliSeconds));
            }
         }
         catch (Exception e)
         {
            AddAlgorithmException(e).Wait();
            return false;
         }

         return true;
      }
      #endregion

      #region Charts initialization
      /// <summary>
      /// Returns the instruments and time-frames used by the algorithm instance.
      /// </summary>
      /// <returns></returns>
      protected override List<Tuple<string, EGranularity>> GetChartsSpecList()
      {
         string specList = "USD_JPY:M15,USD_CAD:M15,EUR_USD:M15,GBP_USD:M15,AUD_USD:M15,NZD_USD:M15";

         string instrumentGranularities = Parameters.GetString("instrumentGranularities") ?? specList;
         string[] instrumentGranularitiesArray = instrumentGranularities.Split(',');

         var chartsSpecList = new List<Tuple<string, EGranularity>>();

         foreach (string instrumentGranularity in instrumentGranularitiesArray)
         {
            string instrument = instrumentGranularity.Split(':')[0];
            string granularity = instrumentGranularity.Split(':')[1];

            chartsSpecList.Add(new Tuple<string, EGranularity>(instrument, (EGranularity)Enum.Parse(typeof(EGranularity), granularity)));
         }

         return chartsSpecList;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      protected override async Task InitializeCharts()
      {
         await base.InitializeCharts();

         //_charts.ToList().ForEach(c => ExportChart(c.Value));
      }
      #endregion

      #region Charts processing
      /// <summary>
      /// Processes all charts after the frame bars in each has been updated.
      /// </summary>
      protected virtual void ProcessCharts()
      {
         if (_thrustDetector == null)
         {
            _thrustDetector = ObjectBase.Container.GetExportedValue<IPatternDetector>("ThrustDetector");
            _thrustDetector.Initialize(Parameters);
         }
         if (_thrustFactory == null)
         {
            _thrustFactory = ObjectBase.Container.GetExportedValue<IPatternFactory>("ThrustFactory");
            _thrustFactory.Initialize(Parameters);
         }

         foreach (Chart chart in _charts.Values)
         {
            Thrust thrust = (Thrust)chart.Patterns.FirstOrDefault(p => p.Type == MACC.Constants.SignalType.Thrust && (p.Active || p.StrategyTransactionID.HasValue));

            if (thrust == null)
            {
               thrust = _thrustDetector.DetectPattern(chart) as Thrust;

               if (thrust != null)
               {
                  if (thrust.Active && thrust.SignalID == 0)
                     HandleNewThrust(chart, thrust);
                  else if (thrust.SignalID > 0)
                     HandleExistingThrust(chart, thrust);
               }
            }
            else
            {
               thrust = (Thrust)_thrustFactory.UpdatePattern(chart, thrust);
               HandleExistingThrust(chart, thrust);
            }
         }
      }

      /// <summary>
      /// Handler for newly discovered thrust patterns.
      /// </summary>
      /// <param name="chart">The chart containing the thrust pattern.</param>
      /// <param name="thrust">The discovered or updated thrust pattern.</param>
      protected virtual async void HandleNewThrust(Chart chart, Thrust thrust)
      {
         // new signal

         // save the signal
         MCE.Signal signal = GetSignalFromThrust(thrust);
         signal = await SubscriptionCaller.Instance().UpdateSignalAsync(signal);

         // place the order
         signal.StrategyTransactionID = await CreateEntryOrder(signal, chart, thrust);

         // share with others
         SubscriptionCaller.Instance().PushSignalToSubscribersAsync(signal);

         // update the thrust
         thrust.SignalID = signal.SignalID;
         thrust.StrategyTransactionID = signal.StrategyTransactionID;
      }

      /// <summary>
      /// Handler for newly updated thrust patterns.
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="thrust"></param>
      protected virtual async void HandleExistingThrust(Chart chart, Thrust thrust)
      {
         if (thrust.StrategyTransactionID.HasValue)
         {
            ManageOrder(chart, thrust);
            return;
         }

         if (!thrust.Active)
            await PurgeThrust(chart, thrust);
      }

      /// <summary>
      /// Updates the last bar in each chart for which a price bar is provided.
      /// Adds new frames if additional new price bars are received.
      /// </summary>
      /// <param name="bars">List of price bars.</param>
      protected virtual void UpdateCharts(List<PriceBar> bars)
      {
         // unset LastFrameUpdated for all charts
         _charts.ToList().ForEach(c => c.Value.LastFrameUpdated = false);

         foreach (PriceBar bar in bars)
         {
            #region logic for update and/or add frames to chart
            // the list of bars received is already sorted-asc by time
            // at least one bar will have a time == lastFrame.Bar.time
            // after updating lastFrame.Bar, all additional bars rcvd should be added as a new frame
            // currently bars.Count is 2
            #endregion

            string chartKey = bar.instrument + ':' + bar.granularity;
            Chart chart = _charts[chartKey];

            var lastFrame = chart.Frames.Last();

            if (lastFrame.Bar.time == bar.time)
            {
               lastFrame.Bar = bar;

               chart.SetIndicatorValues(lastFrame);
               chart.LastFrameUpdated = true;

               continue;
            }

            if (chart.LastFrameUpdated)
            {
               chart.Frames.Add(CreateChartFrame(bar, chart));

               // keep the chart at 100 frames max size
               if (chart.Frames.Count > _chartMaximumFrames) chart.Frames.RemoveAt(0);
            }
         }

         //_charts.ToList().ForEach(c => ExportChart(c.Value, false));
      }
      #endregion

      #region Order entry & management
      /// <summary>
      /// 
      /// </summary>
      /// <param name="signal"></param>
      /// <param name="instrument"></param>
      /// <param name="retracement"></param>
      /// <returns></returns>
      protected virtual async Task<int> CreateEntryOrder(MCE.Signal signal, Chart chart, Thrust thrust)
      {
         StrategyTransaction transaction = null;

         Tuple<double, double, double> orderPrices = GetOrderPrices(chart.HistoricBidAskSpread, signal.Side, thrust);
         double entryPrice = orderPrices.Item1;
         double stopLossPrice = orderPrices.Item2;
         double takeProfitPrice = orderPrices.Item3;

         int tradeUnits = await TradeUnitsCapacity(chart.Instrument, entryPrice, stopLossPrice);

         if (tradeUnits > 0)
         {
            // 12 hours for now .. how should this change? .. read from metaSetting?
            string expiry = MAOE.Utilities.GetTimeAsXmlSerializedUtc(DateTime.UtcNow.AddHours(12));

            string type = "limit";
            string side = signal.Side == MACC.Constants.SignalSide.Buy ? "buy" : "sell";
            int places = ((FibonacciRetracement)thrust.Study).LevelPlaces();

            // create order
            var orderData = new Dictionary<string, string>
                    {
                        {"instrument", chart.Instrument},
                        {"units", tradeUnits.ToString()},
                        {"side", side},
                        {"type", type},
                        {"expiry", expiry},
                        {"price", Math.Round(entryPrice, places).ToString()},
                        {"stopLoss", Math.Round(stopLossPrice, places).ToString()},
                        {"takeProfit", Math.Round(takeProfitPrice, places).ToString()}
                    };

            PostOrderResponse response = await PlaceLimitOrder(orderData);

            if (response.orderOpened != null && response.orderOpened.id > 0)
            {
               MarketMinerOrder orderOpened = new MarketMinerOrder(response.orderOpened);
               orderOpened.SignalID = signal.SignalID;

               transaction = CreateEntryTransaction(chart.Instrument, orderOpened.side, response.time, type, response.price.GetValueOrDefault(), orderOpened.takeProfit, orderOpened.stopLoss, orderOpened.id.ToString());
               transaction = await StrategyCaller.Instance().SaveStrategyTransactionAsync(transaction, MarketMiner.Common.Constants.Brokers.OANDA);
               orderOpened.StrategyTransactionID = transaction.StrategyTransactionID;

               // add entry trans id to the signal
               signal.StrategyTransactionID = transaction.StrategyTransactionID;
               await SubscriptionCaller.Instance().UpdateSignalAsync(signal);

               AddOrUpdateAlgorithmOrder(orderOpened);

               DateTime transactionTime = Convert.ToDateTime(response.time).ToUniversalTime();
               thrust.SetOrUpdatePrices(entryPrice, takeProfitPrice, stopLossPrice, places, transactionTime);
            }
         }
         else
         {
            string missedTradeReason = Utilities.GetMissedTradeReason(tradeUnits);
            AddAlgorithmMessage(string.Format("Missed trade: {0} for signalId: {1}", missedTradeReason, signal.SignalID), true, TraceEventType.Information);
         }

         return transaction == null ? 0 : transaction.StrategyTransactionID;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="orderTransaction"></param>
      /// <param name="thrust"></param>
      /// <param name="retracement"></param>
      /// <returns></returns>
      protected virtual async Task UpdateEntryOrder(StrategyTransaction orderTransaction, Chart chart, Thrust thrust)
      {
         long orderId = Convert.ToInt64(orderTransaction.BrokerTransactionID);

         MarketMinerOrder currentOrder = Orders.FirstOrDefault(o => o.id == orderId) as MarketMinerOrder;

         string side = thrust.Side;

         Tuple<double, double, double> orderPrices = GetOrderPrices(chart.HistoricBidAskSpread, side, thrust);
         double entryPrice = orderPrices.Item1;
         double stopLossPrice = orderPrices.Item2;
         double takeProfitPrice = orderPrices.Item3;

         // round and review
         int places = ((FibonacciRetracement)thrust.Study).LevelPlaces();

         bool updateOrder = false;

         if (currentOrder == null)
            updateOrder = true;
         else
         {
            if (Math.Round(entryPrice, places) != currentOrder.price) updateOrder = true;
            if (Math.Round(stopLossPrice, places) != currentOrder.stopLoss) updateOrder = true;
            if (Math.Round(takeProfitPrice, places) != currentOrder.takeProfit) updateOrder = true;
         }

         if (updateOrder)
         {
            int tradeUnits = await TradeUnitsCapacity(thrust.Instrument, entryPrice, stopLossPrice, true);

            // order size should never increase
            tradeUnits = currentOrder != null ? Math.Min(currentOrder.units, tradeUnits) : tradeUnits;

            if (tradeUnits > 0)
            {
               // 12 hours for now .. how should this change?
               string expiry = MAOE.Utilities.GetTimeAsXmlSerializedUtc(DateTime.UtcNow.AddHours(12));

               // create patch
               var patchData = new Dictionary<string, string>
                    {
                        {"units", tradeUnits.ToString()},
                        {"expiry", expiry},
                        {"price", Math.Round(entryPrice, places).ToString()},
                        {"stopLoss", Math.Round(stopLossPrice, places).ToString()},
                        {"takeProfit", Math.Round(takeProfitPrice, places).ToString()}
                    };

               // what if the fill happens just as execution arrives here?
               // this bit should somehow affirm that the order remains unfilled
               Order updatedOrder = await Rest.PatchOrderAsync(_accountId, orderId, patchData);

               if (updatedOrder != null)
               {
                  MarketMinerOrder mmUpdatedOrder = new MarketMinerOrder(updatedOrder);
                  mmUpdatedOrder.SignalID = thrust.SignalID;
                  mmUpdatedOrder.StrategyTransactionID = orderTransaction.StrategyTransactionID;

                  AddOrUpdateAlgorithmOrder(mmUpdatedOrder);

                  DateTime transactionTime = Convert.ToDateTime(updatedOrder.time).ToUniversalTime();
                  thrust.SetOrUpdatePrices(entryPrice, takeProfitPrice, stopLossPrice, places, transactionTime);
               }
            }
            else
            {
               thrust.Active = false;

               // cancel the order
               await Rest.DeleteOrderAsync(_accountId, orderId);

               // clear the chart
               PurgeThrust(chart, thrust);

               string missedTradeReason = Utilities.GetMissedTradeReason(tradeUnits);
               AddAlgorithmMessage(string.Format("Missed trade: {0} for signalId: {1}", missedTradeReason, thrust.SignalID), true, TraceEventType.Information);
            }
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="tradeId"></param>
      /// <param name="takeProfitPrice"></param>
      /// <param name="stopLossPrice"></param>
      /// <returns></returns>
      protected virtual async Task UpdateOpenTrade(Thrust thrust, long tradeId, double? takeProfitPrice, double? stopLossPrice, int? places)
      {
         // create patch
         Dictionary<string, string> patchData = new Dictionary<string, string>();

         if (takeProfitPrice.GetValueOrDefault() > 0)
            patchData.Add("takeProfit", takeProfitPrice.ToString());

         if (stopLossPrice.GetValueOrDefault() > 0)
            patchData.Add("stopLoss", stopLossPrice.ToString());

         TradeData response = null;

         // send patch
         if (patchData.Count > 0)
         {
            try
            {
               response = await Rest.PatchTradeAsync(_accountId, tradeId, patchData);
            }
            catch (Exception e)
            {
               throw e;
            }

            DateTime transactionTime = Convert.ToDateTime(response.time).ToUniversalTime();
            thrust.SetOrUpdatePrices(null, takeProfitPrice, stopLossPrice, places, transactionTime);
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="thrust"></param>
      /// <param name="retracement"></param>
      protected virtual async void ManageOrder(Chart chart, Thrust thrust)
      {
         StrategyTransaction[] transactions = null;
         StrategyTransaction orderTransaction, fillTransaction, cancelTransaction, exitTransaction;

         // get transaction collection from db
         int orderTransactionID = thrust.StrategyTransactionID.GetValueOrDefault();
         transactions = await StrategyCaller.Instance().GetStrategyTransactionsCollectionAsync(orderTransactionID);
         orderTransaction = transactions.FirstOrDefault(t => t.StrategyTransactionID == orderTransactionID);
         fillTransaction = transactions.FirstOrDefault(t => t.BrokerOrderID != null && t.Type == MACC.Constants.TransactionTypes.OrderFilled);
         cancelTransaction = transactions.FirstOrDefault(t => t.BrokerOrderID != null && t.Type == MACC.Constants.TransactionTypes.OrderCancel);
         exitTransaction = transactions.FirstOrDefault(t => t.BrokerTradeID != null && t.Type != MACC.Constants.TransactionTypes.TradeUpdate);

         bool purgeInActiveThrust = true;

         if (fillTransaction == null)
         {
            if (cancelTransaction != null)
            {
               // order cancelled

               thrust.Active = false;
            }
            else
            {
               // order open

               if (!thrust.Active)
               {
                  // cancel the order
                  // the cancel transaction will be written to the db by the event stream handler
                  await Rest.DeleteOrderAsync(_accountId, Convert.ToInt64(orderTransaction.BrokerTransactionID));
               }
               else
               {
                  if (thrust.FocusChanged() || (thrust.FillZoneReached() && thrust.TakeProfitZoneReached()))
                  {
                     AddAlgorithmMessage(string.Format("UPDATE ORDER: {0}: FCH-{1}: FZR-{2}: TPR-{3}", orderTransaction.Instrument, thrust.FocusChanged(), thrust.FillZoneReached(), thrust.TakeProfitZoneReached()));
                     UpdateEntryOrder(orderTransaction, chart, thrust);
                  }
                  
               }
            }
         }
         else
         {
            if (exitTransaction == null)
            {
               // order filled

               purgeInActiveThrust = false;
               FibonacciRetracement retracement = (FibonacciRetracement)thrust.Study;

               #region get open trade info
               long tradeId = Convert.ToInt64(fillTransaction.BrokerTransactionID);
               Frame fillFrame = chart.Frames.LastOrDefault(f => Convert.ToDateTime(f.Bar.time).ToUniversalTime() <= fillTransaction.Time);
               int fillIndex = chart.Frames.IndexOf(fillFrame);
               #endregion

               #region get takeProfitPrice and stopLossPrice
               int profitWaitPeriods = Parameters.GetInteger("thrustProfitWaitPeriods") ?? 6;
               thrust.ProfitWindowClosed = (fillIndex + profitWaitPeriods) < chart.Frames.Count;

               IPriceBar lastBar = chart.Frames.Last().Bar;
               bool hasProfit = orderTransaction.Side == MACC.Constants.SignalSide.Buy ? lastBar.closeMid > fillTransaction.Price : lastBar.closeMid < fillTransaction.Price;

               #region stopLossPrice & takeProfitPrice logic
               // adjust stopLossPrice and takeProfitPrice
               // if a buy ...
               //    move the stop to the lower of the .500 fib price or [patern lowMid price set by the post-fill price action]
               //    move the profit target to 1 or 2 pips under .618 * (thrust.FocusPrice - [pattern lowMid price set by the post-fill price action])
               // if a sell ...
               //    move the stop to the higher of the .500 fib price or [patern highMid price set by the post-fill price action]
               //    move the profit target to 1 or 2 pips above .618 * ([pattern lowMid price set by the post-fill price action] - thrust.FocusPrice)
               #endregion

               double? takeProfitPrice = GetAdjustedTakeProfitPrice(chart, thrust.Side, retracement);
               AddAlgorithmMessage(string.Format("GET SLP: {0}: TPZ-{1}: PWC-{2}: XTP-{3}: CLP-{4}: PFT-{5}", fillTransaction.Instrument, thrust.TakeProfitZoneReached(), thrust.ProfitWindowClosed, retracement.ExtremaPrice, lastBar.closeMid, hasProfit));
               double? stopLossPrice = GetAdjustedStopLossPrice(chart, thrust.Side, thrust, hasProfit);
               #endregion

               #region kill or update the trade
               // not profitable && beyond r500 .. kill it
               if (stopLossPrice.GetValueOrDefault() == -1)
               {
                  thrust.Active = false;
                  purgeInActiveThrust = true;

                  try
                  {
                     await Rest.DeleteTradeAsync(_accountId, tradeId);
                  }
                  catch (Exception e)
                  {
                     AddAlgorithmMessage(string.Format("CLOSE TRADE {0} Failed: {1}", tradeId, e.Message), true, TraceEventType.Error);
                  }
               }
               else
               {
                  if ((takeProfitPrice ?? thrust.TakeProfitPrice) != thrust.TakeProfitPrice || (stopLossPrice ?? thrust.StopLossPrice) != thrust.StopLossPrice)
                  {
                     AddAlgorithmMessage(string.Format("UPDATE TRADE: {0}: TPZ-{1}: PWC-{2}: XTP-{3}: CLP-{4}: TP-{5}: TTP-{6}: SL-{7}: TSL-{8}", fillTransaction.Instrument, thrust.TakeProfitZoneReached(), thrust.ProfitWindowClosed, retracement.ExtremaPrice, lastBar.closeMid, takeProfitPrice, thrust.TakeProfitPrice, stopLossPrice, thrust.StopLossPrice));
                     UpdateOpenTrade(thrust, tradeId, takeProfitPrice, stopLossPrice, retracement.LevelPlaces());
                  }
                     
               }
               #endregion
            }
            else
            {
               // trade closed

               #region about closed trades
               // if coll includes a stopLossFilled or takeProfitFilled ..
               // set thrust.Active = false 
               // this be done server side when the strategyTransaction from the stream is saved
               // the signal should be found on the server and signal.Active should be set to false
               // do it here also
               #endregion

               thrust.Active = false;
            }
         }

         if (!thrust.Active && purgeInActiveThrust)
            await PurgeThrust(chart, thrust);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="side"></param>
      /// <param name="retracement"></param>
      /// <returns></returns>
      protected virtual Tuple<double, double, double> GetOrderPrices(double spread, string side, Thrust thrust)
      {
         double entryPrice, stopLossPrice;

         FibonacciRetracement retracement = (FibonacciRetracement)thrust.Study;

         if (side == MACC.Constants.SignalSide.Buy)
         {
            entryPrice = retracement.LevelPrice(FibonacciLevel.R382) + (0.75 * spread);
            stopLossPrice = retracement.LevelPrice(FibonacciLevel.R618) - (0.75 * spread);

            if (thrust.FillZoneReached() && thrust.TakeProfitZoneReached())
               stopLossPrice = retracement.LevelPrice(FibonacciLevel.R500) - (0.75 * spread);
         }
         else
         {
            entryPrice = retracement.LevelPrice(FibonacciLevel.R382) - (0.75 * spread);
            stopLossPrice = retracement.LevelPrice(FibonacciLevel.R618) + (0.75 * spread);

            if (thrust.FillZoneReached() && thrust.TakeProfitZoneReached())
               stopLossPrice = retracement.LevelPrice(FibonacciLevel.R500) + (0.75 * spread);
         }

         int multiplier = side == MACC.Constants.SignalSide.Buy ? 1 : -1;
         double r382Price = retracement.LevelPrice(FibonacciLevel.R382);
         double takeProfitPrice = r382Price + (0.618 * Math.Abs(retracement.FocusPrice - r382Price) * multiplier);

         var orderPrices = Tuple.Create<double, double, double>(entryPrice, stopLossPrice, takeProfitPrice);

         return orderPrices;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="retracement"></param>
      /// <param name="side"></param>
      /// <returns></returns>
      protected virtual double? GetAdjustedTakeProfitPrice(Chart chart, string side, FibonacciRetracement retracement)
      {
         #region logic
         // if a buy ...
         //    move the profit target to 1 or 2 pips under .618 * (thrust.FocusPrice - [pattern lowMid price set by the post-fill price action])
         // if a sell ...
         //    move the profit target to 1 or 2 pips above .618 * ([pattern lowMid price set by the post-fill price action] - thrust.FocusPrice)
         #endregion

         if (_takeProfitPriceCalculator == null)
            _takeProfitPriceCalculator = ObjectBase.Container.GetExportedValue<ITakeProfitPriceCalculator>("ThrustTakeProfitPriceCalculator");

         Dictionary<string, object> state = new Dictionary<string, object>();
         state.Add("side", side);
         state.Add("retracement", retracement);

         return _takeProfitPriceCalculator.GetAdjustedTakeProfitPrice(chart, Parameters, state);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="retracement"></param>
      /// <param name="side"></param>
      /// <param name="hasProfit"></param>
      /// <param name="profitWindowClosed"></param>
      /// <returns></returns>
      protected virtual double? GetAdjustedStopLossPrice(Chart chart, string side, Thrust thrust, bool hasProfit)
      {
         if (_stopLossPriceCalculator == null)
            _stopLossPriceCalculator = ObjectBase.Container.GetExportedValue<IStopLossPriceCalculator>("ThrustStopLossPriceCalculator");

         Dictionary<string, object> state = new Dictionary<string, object>();
         state.Add("side", side);
         state.Add("thrust", thrust);
         state.Add("hasProfit", hasProfit);

         return _stopLossPriceCalculator.GetAdjustedStopLossPrice(chart, Parameters, state);
      }
      #endregion

      #region Utilities
      /// <summary>
      /// Creates a Signal object from the supplied Thrust object.
      /// </summary>
      /// <param name="thrust"></param>
      /// <returns>The Signal object created.</returns>
      protected virtual MCE.Signal GetSignalFromThrust(Thrust thrust)
      {
         // find the productID
         var product = SubscriptionCaller.Instance().GetProductByName(thrust.GetProductName());
         if (product == null)
            throw new NotFoundException(string.Format("Product ({0}) not found for thrust signal.", thrust.GetProductName()));

         // save the signal
         var signal = new MCE.Signal() { ProductID = product.ProductID };
         signal.InjectWith(thrust);

         return signal;
      }

      /// <summary>
      /// Clears the state items associated with an inactive thrust.
      /// </summary>
      /// <param name="chart">The chart containing the thrust pattern to be purged.</param>
      /// <param name="thrust">The inactive thrust pattern to be purged.</param>
      /// <returns></returns>
      protected virtual async Task PurgeThrust(Chart chart, Thrust thrust)
      {
         // kill and save the signal
         MCE.Signal signal = await SubscriptionCaller.Instance().GetSignalAsync(thrust.SignalID);
         signal.Active = false;
         await SubscriptionCaller.Instance().UpdateSignalAsync(signal);

         // clear the chart
         chart.Patterns.RemoveAll(p => p.Type == MACC.Constants.SignalType.Thrust);
         Orders.Remove(Orders.FirstOrDefault(o => ((MarketMinerOrder)o).SignalID == signal.SignalID));
         Trades.Remove(Trades.FirstOrDefault(t => ((MarketMinerTrade)t).SignalID == signal.SignalID));
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="chart"></param>
      /// <param name="initialize"></param>
      /// <returns></returns>
      public async Task ExportChart(Chart chart, bool initialize = true)
      {
         await Task.Run(() =>
         {
            try
            {
               //before your loop
               StringBuilder csv = new StringBuilder();

               Func<Frame, string> getDataLine = frame =>
               {
                  string date = frame.Bar.time;
                  string open = frame.Bar.openMid.ToString();
                  string high = frame.Bar.highMid.ToString();
                  string low = frame.Bar.lowMid.ToString();
                  string close = frame.Bar.closeMid.ToString();
                  string volume = frame.Bar.volume.ToString();
                  string dma = frame.Indicators.First(k => k.Type == IndicatorType.DisplacedMovingAverage).Value.GetValueOrDefault().ToString();
                  //string osc = frame.Indicators.First(k => k.Type == IndicatorType.DetrendedOscillator).Value.ToString();

                  return string.Format("{0},{1},{2},{3},{4},{5},{6}", date, open, high, low, close, volume, dma);
               };

               string filePath = @"C:\Users\Osita\SourceCode\MarketMiner\Development\MarketMiner.Web.JS\";
               filePath += chart.Instrument;
               filePath += ".csv";

               if (initialize)
               {
                  // header row
                  csv.AppendLine("Date,Open,High,Low,Close,Volume,DMA");

                  foreach (Frame frame in chart.Frames)
                  {
                     csv.AppendLine(getDataLine(frame));
                  }

                  File.WriteAllText(filePath, csv.ToString());
               }
               else
               {
                  int frameCount = chart.Frames.Count;

                  foreach (Frame frame in chart.Frames.Skip(frameCount - 2))
                  {
                     csv.AppendLine(getDataLine(frame));
                  }

                  string[] lines = File.ReadAllLines(filePath);
                  File.WriteAllLines(filePath, lines.Take(lines.Length - 2).ToArray());

                  File.AppendAllText(filePath, csv.ToString());
               }
            }
            catch (Exception e)
            {
               Exception ex = new Exception(string.Format("An error occured while writing file: {0}.csv.", chart.Instrument), e);
               AddAlgorithmException(ex).Wait();

               // let's not shut things down when this faults .. for now
               //throw ex;
            }
         });
      }
      #endregion

      #region Streaming data handlers
      /// <summary>
      /// Handler for OANDA rate data received from data stream.
      /// </summary>
      /// <param name="data">The price tick data received.</param>
      protected override void OnRateReceived(RateStreamResponse data)
      {
         base.OnRateReceived(data);
      }

      /// <summary>
      /// Handler for OANDA event data received from event stream.
      /// </summary>
      /// <param name="data">The Event data received.</param>
      protected virtual void OnEventReceived(Event data)
      {
         if (data == null)
            throw new ApplicationException("Event stream returned null.");

         if (!data.IsHeartbeat())
         {
            Transaction t = data.transaction;

            SaveStrategyTransaction(t);

            // tell
            string messageEnd = t.type == MACC.Constants.TransactionTypes.OrderCancel ? t.reason : t.instrument;
            string message = string.Format("TransId: {0}, {1}, {2} - {3}", t.id, t.time, t.type, messageEnd);
            AddAlgorithmMessage(message);

            MAOE.Utilities.ReleaseEvent();
         }

         // send an hourly smoke signal
         if (DateTime.Now.Minute == 10 && CoreUtilities.Between<int>(DateTime.Now.Second, 0, 30))
            PostAlgorithmStatusNotification("BreadAndButterAlgorithm01");
      }

      protected override void OnConnectionStatusChangedAction(ApiConnectionStatus status)
      {
         if (_instance.Status == (short?)AlgorithmStatus.Running)
         {
            if (status == ApiConnectionStatus.Faulted)
            {
               _chartsInitialized = false;
            }
            else if (status == ApiConnectionStatus.Recovered)
            {

            }
            else if (status == ApiConnectionStatus.Connected)
            {
               // api connection restored
               if (_lastConnectionStatus == ApiConnectionStatus.Recovered && !_chartsInitialized)
               {
                  InitializeCharts();
               }
            }
         }
      }
      #endregion
   }
}
