using MarketMiner.Algorithm.Common;
using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.OANDA.Common;
using MarketMiner.Api.Client.OANDA.Data.DataModels;
using MarketMiner.Api.Common.Contracts;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.Extensions;
using MarketMiner.Api.OANDA.Extensions.Classes;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications.Requests;
using MarketMiner.Client.Common;
using MarketMiner.Client.Entities;
using MarketMiner.Client.Proxies.ServiceCallers;
using MarketMiner.Common.Enums;
using N.Core.Common.Contracts;
using P.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiOANDA = MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using MAC = MarketMiner.Algorithm.Common;
using MACC = MarketMiner.Api.Client.Common;
using MAOE = MarketMiner.Api.OANDA.Extensions;
using MC = MarketMiner.Common;
using MCE = MarketMiner.Client.Entities;

namespace MarketMiner.Algorithm.OANDA.Common
{
   public abstract class OANDAAlgorithmBase : TradingAlgorithmBase
   {
      public OANDAAlgorithmBase(IEventLogger logger)
         : base(logger)
      {
      }

      #region start-up
      public override async Task<bool> Start()
      {
         bool okToStart = await base.Start();

         string type = MC.Constants.MetaSettings.Types.Api;
         string environment = MetadataHelper.GetSettingAsString(type, "OANDA_Environment") ?? "Practice";
         string token = MetadataHelper.GetSettingAsString(type, string.Format("OANDA_{0}_Token", environment));
         string algorithmId = _strategy.StrategyID < 10 ? "0" + _strategy.StrategyID.ToString() : _strategy.StrategyID.ToString();
         string code = string.Format("OANDA_{0}_Algorithm{1}_Account", environment, algorithmId);
         _accountId = MetadataHelper.GetSettingAsInteger(type, code).GetValueOrDefault();

         Credentials.SetCredentials((EEnvironment)Enum.Parse(typeof(EEnvironment), environment), token, _accountId);

         okToStart = await UpdateStrategyTransactions();
         okToStart = await UpdateOrdersAndTrades();

         return okToStart;
      }
      #endregion

      #region charts
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      protected virtual List<Tuple<string, EGranularity>> GetChartsSpecList()
      {
         var chartsSpecList = new List<Tuple<string, EGranularity>>() 
         { 
            Tuple.Create<string, EGranularity>(MAOE.Constants.Instruments.USDJPY, EGranularity.M15),
            Tuple.Create<string, EGranularity>(MAOE.Constants.Instruments.EURUSD, EGranularity.M15),
            Tuple.Create<string, EGranularity>(MAOE.Constants.Instruments.AUDUSD, EGranularity.M15),
            Tuple.Create<string, EGranularity>(MAOE.Constants.Instruments.USDCAD, EGranularity.M15)
         };

         return chartsSpecList;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      protected virtual async Task InitializeCharts()
      {
         AddAlgorithmMessage(string.Format("{0} charts are initializing ...", _shortClassName), true, TraceEventType.Information);

         bool okToInitializeCharts = false;

         List<PriceBar> bars = null;

         while (!okToInitializeCharts)
         {
            bars = await RatesDataSource.Instance.GetPriceBars(GetChartsSpecList(), _chartInitialFrames);

            if (bars != null && bars.Count % 100 == 0)
            {
               okToInitializeCharts = true;

               _charts.ToList().ForEach(c => c.Value.Initialize());

               foreach (var bar in bars)
               {
                  string chartKey = bar.instrument + ":" + bar.granularity;
                  Chart chart = _charts[chartKey];
                  chart.Frames.Add(CreateChartFrame(bar, chart));
               }

               SetHistoricBidAskSpreads();

               _chartsInitialized = true;
            }
            else
            {
               await Task.Delay(TimeSpan.FromMinutes(5));
            }
         }

         AddAlgorithmMessage(string.Format("{0} charts ({1}) are initialized.", _shortClassName, _charts.Count), true, TraceEventType.Information);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="instrument"></param>
      /// <returns></returns>
      protected virtual double GetHistoricBidAskSpreads(string instrument)
      {
         double spread = 0;

         var chartSpec = new List<Tuple<string, EGranularity>>() 
               { 
                  Tuple.Create<string, EGranularity>(instrument, EGranularity.H1)
               };

         List<PriceBar> bars = RatesDataSource.Instance.GetPriceBars(chartSpec, 4800, ECandleFormat.bidask).Result;

         for (int i = 0; i < 4800; i += 240)
         {
            double historicSpread = bars.Skip(i).Take(240).Average(b => b.closeAsk - b.closeBid);

            spread = spread == 0 ? historicSpread : Math.Min(spread, historicSpread);
         }

         return spread;
      }

      /// <summary>
      /// 
      /// </summary>
      protected virtual void SetHistoricBidAskSpreads()
      {
         bool historicBidAskSpreadsAreSet = _charts.ToList().All(c => c.Value.HistoricBidAskSpread > 0);

         if (!historicBidAskSpreadsAreSet)
         {
            foreach (Chart chart in _charts.Values)
            {
               double spread = GetHistoricBidAskSpreads(chart.Instrument);

               chart.HistoricBidAskSpread = spread;
            }
         }
      }
      #endregion

      #region orders
      /// <summary>
      /// These keys are required for every OANDA order
      /// </summary>
      List<string> _requiredKeys = new List<string>() { "instrument", "units", "side", "type", "expiry", "price", "stopLoss", "takeProfit" };

      protected async Task PlaceTestMarketOrder()
      {
         // create new market order
         var order = new Dictionary<string, string> 
         {
            {"instrument", "USD_JPY"},
            {"units", "1"},
            {"side", "buy"},
            {"type", "market"},
            {"price", "1.0"}
         };

         await PlaceMarketOrder(order);
      }

      protected virtual Dictionary<string, string> CreateOrder(
         string instrument, string units, string side, string type, string expiry, string price, string stopLoss, string takeProfit)
      {
         // create order
         var orderData = new Dictionary<string, string>
                    {
                        {"instrument", instrument},
                        {"units", units},
                        {"side", side},
                        {"type", type},
                        {"expiry", expiry},
                        {"price", price},
                        {"stopLoss", stopLoss},
                        {"takeProfit", takeProfit}
                    };

         ValidateOrder(orderData);

         return orderData;
      }

      protected Dictionary<string, string> CreateOrderPatch(double? entryPrice, double? takeProfitPrice, double? stopLossPrice)
      {
         // create patch
         Dictionary<string, string> patchData = new Dictionary<string, string>();

         if (entryPrice.GetValueOrDefault() > 0)
            patchData.Add("price", entryPrice.ToString());

         if (takeProfitPrice.GetValueOrDefault() > 0)
            patchData.Add("takeProfit", takeProfitPrice.ToString());

         if (stopLossPrice.GetValueOrDefault() > 0)
            patchData.Add("stopLoss", stopLossPrice.ToString());

         return patchData;
      }

      protected virtual async Task<PostOrderResponse> PlaceMarketOrder(Dictionary<string, string> order)
      {
         // do market stuff

         // place order
         PostOrderResponse response = await PlaceOrder("market", order);

         return response;
      }

      protected virtual async Task<PostOrderResponse> PlaceLimitOrder(Dictionary<string, string> order)
      {
         // do limit stuff

         // place order
         PostOrderResponse response = await PlaceOrder("limit", order);

         return response;
      }

      protected virtual async Task<PostOrderResponse> PlaceOrder(string type, Dictionary<string, string> order)
      {
         // make sure type is correct
         if (!order.ContainsKey("type"))
            order.Add("type", type);
         else
            order["type"] = type;

         ValidateOrder(order);

         PostOrderResponse response = null;

         if (!await Helpers.IsMarketHalted())
            response = await Rest.PostOrderAsync(_accountId, order);

         return response;
      }

      protected virtual bool ValidateOrder(Dictionary<string, string> order)
      {
         Action<string> throwException = message => { throw new ArgumentException(message); };

         _requiredKeys.FirstOrDefault(key =>
         {
            if (!order.ContainsKey(key))
            {
               throwException(string.Format("OrderData does not contain required key: ", key));
               return true;
            }
            return false;
         });

         if (new[] { "buy", "sell" }.Contains(order["side"]) == false)
            throwException(string.Format("Order side: {0} is invalid.", order["side"]));

         double price = Convert.ToDouble(order["price"]);
         double stopLoss = Convert.ToDouble(order["stopLoss"]);
         double takeProfit = Convert.ToDouble(order["takeProfit"]);

         if (order["side"] == "buy")
         {
            if (price <= stopLoss)
               throwException(string.Format("Order price: {0} is less than or equal to StopLoss price: {1}", price, stopLoss));
            else if (price >= takeProfit)
               throwException(string.Format("Order price: {0} is greater than or equal to TakeProft price: {1}", price, takeProfit));
         }

         if (order["side"] == "sell")
         {
            if (price >= stopLoss)
               throwException(string.Format("Order price: {0} is greater than or equal to StopLoss price: {1}", price, stopLoss));
            else if (price <= takeProfit)
               throwException(string.Format("Order price: {0} is less than or equal to TakeProft price: {1}", price, takeProfit));
         }

         return true;
      }
      #endregion

      #region trades
      protected virtual async Task<int> TradeUnitsCapacity(string instrument, double entryPrice, double stopLossPrice, bool forOrderUpdate = false)
      {
         ApiOANDA.Account oandaAccount = await Rest.GetAccountDetailsAsync(_accountId);
         MarketMinerAccount account = new MarketMinerAccount(oandaAccount);

         List<Order> openOrders = await Rest.GetOrderListAsync(_accountId);

         if (!forOrderUpdate)
         {
            if (openOrders.FirstOrDefault(o => o.instrument == instrument) != null)
               return MAC.Constants.MissedTradeReason.AccountOpenOrder;
            //if (account.HasOpenOrders())
            //   return MAC.Constants.MissedTradeReason.AccountOpenOrder;
            //if (account.HasOpenTrades())
            //   return MAC.Constants.MissedTradeReason.AccountOpenTrade;
            if (!account.HasBalance())
               return MAC.Constants.MissedTradeReason.AccountZeroBalance;
            if (!account.HasMarginAvail())
               return MAC.Constants.MissedTradeReason.AccountInsufficientMargin;
         }

         #region How to compute trade risk
         /*
            Example 1:
            You see that the rate for EUR/USD is 0.9517/22 and decide to sell 10,000 EUR. Your trade is executed at 0.9517.
            10,000 EUR * 0.9517= 9,517.00 USD
            You sold 10,000 EUR and bought 9,517.00 USD.
            After you trade, the market rate of EUR/USD decreases to EUR/USD=0.9500/05. You then buy back 10,000 EUR at 0.9505.
            10,000 EUR *0.9505= 9,505.00 USD
            You sold 10,000 EUR for 9,517 USD and bought 10,000 back for 9,505. The difference is your profit:
            9,517.00-9,505.00= $12.00 USD

            Example 2:
            You see that the rate for USD/JPY is 115.00/05 and decide to buy 10,000 USD. Your trade is executed at 115.05.
            10,000 USD*115.05= 1,150,500 JPY
            You bought 10,000 USD and sold 1,150,500 JPY.
            The market rate of USD/JPY falls to 114.45/50. You decide to sell back 10,000 USD at 114.45.
            10,000 USD*114.45=1,144,500 JPY
            You bought 10,000 USD for 1,150,500 JPY and sold 10,000 USD back for 1,144,500 JPY. 
            The difference is your loss and is calculated as follows:
               1,150,500-1,144,500= 6,000 JPY. Note that your loss is in JPY and must be converted back to dollars.
            To calculate this amount in USD:
            6,000 JPY/ 114.45 = $52.42 USD or 
            6,000 *1/114.45=$52.42          
          */
         #endregion

         int tradeUnits = 0;
         double balance, marginAvail, marginRate, maxVaR, perUnitVaR;

         double.TryParse(account.balance, out balance);
         double.TryParse(account.marginAvail, out marginAvail);
         double.TryParse(account.marginRate, out marginRate);

         maxVaR = balance * Parameters.GetDouble("maxTradeValueAtRisk") ?? 0.01;
         perUnitVaR = GetPerUnitVaR(instrument, entryPrice, stopLossPrice);
         tradeUnits = Convert.ToInt32(maxVaR / perUnitVaR);

         return tradeUnits;
      }

      protected virtual double GetPerUnitVaR(string instrument, double entryPrice, double stopLossPrice)
      {
         double perUnitVaR = 0;

         if (instrument.StartsWith("USD"))
            perUnitVaR = (entryPrice - stopLossPrice) / stopLossPrice;
         else if (instrument.EndsWith("USD"))
            perUnitVaR = entryPrice - stopLossPrice;
         else
         {
            switch (instrument)
            {
               case MAOE.Constants.Instruments.USDJPY:
                  perUnitVaR = (entryPrice - stopLossPrice) / stopLossPrice;
                  break;
               default:
                  throw new ArgumentException(string.Format("Unrecognized or invalid instrument: ", instrument));
            }
         }

         return Math.Abs(perUnitVaR);
      }

      #region trade tests
      protected bool _tradeTestCompleted;

      protected async Task RunTradesTestAsync()
      {
         if (!_tradeTestCompleted)
         {
            // trade tests
            await PlaceTestMarketOrder();

            // get list of open trades
            var openTrades = await Rest.GetTradeListAsync(Security.Account.PracticeAccount);
            if (openTrades.Count > 0)
            {
               // get details for a trade
               var tradeDetails = await Rest.GetTradeDetailsAsync(Security.Account.PracticeAccount, openTrades[0].id);
               bool detailsRetrieved = tradeDetails.id > 0 && tradeDetails.price > 0 && tradeDetails.units != 0;

               // Modify an open trade
               var request = new Dictionary<string, string>
                    {
                        {"stopLoss", "0.4"}
                    };
               var modifiedDetails = await Rest.PatchTradeAsync(Security.Account.PracticeAccount, openTrades[0].id, request);
               bool tradeModified = modifiedDetails.id > 0 && Math.Abs(modifiedDetails.stopLoss - 0.4) < float.Epsilon;

               if (!await Helpers.IsMarketHalted())
               {
                  // close an open trade
                  var closedDetails = await Rest.DeleteTradeAsync(Security.Account.PracticeAccount, openTrades[0].id);
                  bool tradeClosed = closedDetails.id > 0;
               }
               else
               {
                  //
               }
            }
            else
            {
               //
            }
         }

         _tradeTestCompleted = true;
      }
      #endregion

      #endregion

      #region transactions
      protected StrategyTransaction CreateEntryTransaction(string instrument, string side, string time, string type, double price, double? takeProfit, double? stopLoss, string id)
      {
         return CreateStrategyTransaction(instrument, side, time, type, price, takeProfit, stopLoss, id, null, null);
      }

      protected StrategyTransaction CreateOrderUpdateTransaction(string instrument, string side, string time, string type, double price, double? takeProfit, double? stopLoss, string id, string orderId)
      {
         return CreateStrategyTransaction(instrument, side, time, type, price, takeProfit, stopLoss, id, orderId, null);
      }

      protected StrategyTransaction CreateFillTransaction(Transaction t)
      {
         StrategyTransaction strategyTransaction = CreateStrategyTransaction(t.instrument, t.side, t.time, t.type, t.price, t.takeProfitPrice, t.stopLossPrice, t.id.ToString(), t.orderId.ToString(), null);

         IOrder tradeOrder = Orders.FirstOrDefault(o => o.id == t.orderId);
         if (tradeOrder != null)
         {
            ((MarketMinerOrder)tradeOrder).Filled = true;

            MarketMinerTrade tradeOpened = new MarketMinerTrade(t.tradeOpened);
            tradeOpened.SignalID = ((MarketMinerOrder)tradeOrder).SignalID;
            tradeOpened.StrategyTransactionID = strategyTransaction.StrategyTransactionID;
            AddOrUpdateAlgorithmTrade(tradeOpened);
         }

         return strategyTransaction;
      }

      protected StrategyTransaction CreateTradeUpdateTransaction(Transaction t, string instrument, string side, string time, string type, double price, double? takeProfit, double? stopLoss, string id, string tradeId)
      {
         return CreateStrategyTransaction(instrument, side, time, type, price, takeProfit, stopLoss, id, null, tradeId);
      }

      protected StrategyTransaction CreateExitTransaction(Transaction t, string instrument, string side, string time, string type, double price, string id, string tradeId)
      {
         ITrade openTrade = Trades.FirstOrDefault(ot => ot.id == t.tradeId);
         if (openTrade != null)
         {
            ((MarketMinerTrade)openTrade).Closed = true;
         }

         return CreateStrategyTransaction(instrument, side, time, type, price, null, null, id, null, tradeId);
      }

      protected StrategyTransaction CreateOrderCancelTransaction(Transaction t, string instrument, string side, string time, string type, double price, string id, string orderId, string reason)
      {
         IOrder openOrder = Orders.FirstOrDefault(o => o.id == t.orderId);
         if (openOrder != null)
         {
            ((MarketMinerOrder)openOrder).Cancelled = true;
         }

         if (t.reason == MAOE.Constants.OrderCancelReasons.OrderFilled)
            return null;

         return CreateStrategyTransaction(t.instrument, side, time, type, price, null, null, id, orderId, null);
      }

      protected virtual StrategyTransaction CreateStrategyTransaction(string instrument, string side, string time, string type, double price, double? takeProfit, double? stopLoss, string id, string orderId, string tradeId)
      {
         StrategyTransaction strategyTransaction = new StrategyTransaction()
         {
            BrokerID = 0, // this will be updated server side
            BrokerTransactionID = id,
            BrokerOrderID = orderId,
            BrokerTradeID = tradeId,
            AccountID = Credentials.GetDefaultCredentials().DefaultAccountId.ToString(),
            StrategyID = _strategy.StrategyID,
            Instrument = instrument,
            Side = string.IsNullOrEmpty(side)
               ? MACC.Constants.SignalSide.None
               : side.ToLower() == "buy" ? MACC.Constants.SignalSide.Buy : MACC.Constants.SignalSide.Sell,
            Time = Convert.ToDateTime(time).ToUniversalTime(),
            Type = type.ToUpper(),
            Price = price,
            TakeProfit = takeProfit,
            StopLoss = stopLoss
         };

         return strategyTransaction;
      }

      protected virtual StrategyTransaction SaveStrategyTransaction(Transaction t)
      {
         // create a new record for oanda transactions
         StrategyTransaction transaction = null;

         if (!CoreUtilities.InList<string>(t.type
            , MACC.Constants.TransactionTypes.LimitOrderCreate
            , MACC.Constants.TransactionTypes.MarketIfTouchedOrderCreate
            , MACC.Constants.TransactionTypes.MarketOrderCreate))
         {
            if (t.tradeOpened != null) // fill
               transaction = CreateFillTransaction(t);
            else if (t.type == MACC.Constants.TransactionTypes.OrderCancel)
               transaction = CreateOrderCancelTransaction(t, t.instrument, t.side, t.time, t.type, t.price, t.id.ToString(), t.orderId.ToString(), t.reason);
            else if (t.type == MACC.Constants.TransactionTypes.OrderUpdate)
               transaction = CreateOrderUpdateTransaction(t.instrument, t.side, t.time, t.type, t.price, t.takeProfitPrice, t.stopLossPrice, t.id.ToString(), t.orderId.ToString());
            else if (t.type == MACC.Constants.TransactionTypes.TradeUpdate)
               transaction = CreateTradeUpdateTransaction(t, t.instrument, t.side, t.time, t.type, t.price, t.takeProfitPrice, t.stopLossPrice, t.id.ToString(), t.tradeId.ToString());
            else if (t.tradeId > 0
               || CoreUtilities.InList<string>(t.type
                  , MACC.Constants.TransactionTypes.StopLossFilled
                  , MACC.Constants.TransactionTypes.TakeProfitFilled
                  , MACC.Constants.TransactionTypes.TradeClose)) // close      
               transaction = CreateExitTransaction(t, t.instrument, t.side, t.time, t.type, t.price, t.id.ToString(), t.tradeId.ToString());

            if (transaction != null)
               StrategyCaller.Instance().SaveStrategyTransactionAsync(transaction, MarketMiner.Common.Constants.Brokers.OANDA);
         }

         return transaction;
      }

      private List<Transaction> _faultMissedTransactions = new List<Transaction>();

      protected async Task<bool> UpdateStrategyTransactions()
      {
         AddAlgorithmMessage("Retrieving latest OANDA transactions ...", false, TraceEventType.Information);

         bool finished = false;

         try
         {
            // get from database the most recent transaction received from oanda
            StrategyTransaction lastTransaction = StrategyCaller.Instance().GetStrategyTransactions(1, true).FirstOrDefault();

            int newCount = 0;

            if (lastTransaction != null)
            {
               _faultMissedTransactions.Clear();

               // get from oanda all transactions after the most recent received
               int requestCount = 500;
               string minId = lastTransaction.BrokerTransactionID;

               while (!finished)
               {
                  var parameters = new Dictionary<string, string> 
                        {
                           {"minId", minId},
                           {"count", requestCount.ToString()}
                        };

                  List<Transaction> transactions = await Rest.GetTransactionListAsync(_accountId, parameters);

                  if (transactions.Count > 0)
                  {
                     if (transactions.Exists(t => t.id.ToString() == minId))
                        transactions.RemoveAll(t => t.id.ToString() == minId);

                     if (transactions.Count > 0)
                     {
                        _faultMissedTransactions.AddRange(transactions);

                        newCount += transactions.Count;

                        // sort ascending
                        transactions.Sort((t1, t2) => t1.id.CompareTo(t2.id));

                        // save to the db
                        transactions.ForEach(t => SaveStrategyTransaction(t));

                        minId = transactions.Max(t => t.id).ToString();

                        // oanda limit: 1 per 60 secs.
                        // http://developer.oanda.com/rest-live/transaction-history/#pagination
                        if (transactions.Count < 499)
                           finished = true;
                        else
                           await Task.Delay(TimeSpan.FromSeconds(70));
                     }
                     else
                        finished = true;
                  }
               }
            }

            AddAlgorithmMessage(string.Format("{0} new OANDA transactions retrieved.", newCount), false, TraceEventType.Information);

            return true;
         }
         catch (Exception e)
         {
            throw new Exception("Could not retrieve and save latest OANDA transactions.", e);
         }
      }
      #endregion

      #region event handlers
      /// <summary>
      /// Handler for OANDA rate data received from data stream.
      /// </summary>
      /// <param name="data">The price tick data received.</param>
      protected virtual void OnRateReceived(RateStreamResponse data)
      {
         if (!data.IsHeartbeat())
         {
            if (_chartsInitialized)
            {
               string instrument = data.tick.instrument;
               Chart instrumentChart = _charts.FirstOrDefault(c => c.Value.Instrument == instrument).Value;
               IPriceBar lastFrameBar = instrumentChart.Frames.Last().Bar;

               double bidPrice = data.tick.bid;
               double askPrice = data.tick.ask;
               double midPrice = (data.tick.ask - data.tick.bid) / 2;

               if (Convert.ToDateTime(data.tick.time).ToUniversalTime() < instrumentChart.CreateNewFrameTime)
               {
                  lastFrameBar.volume++;

                  lastFrameBar.closeBid = bidPrice;
                  lastFrameBar.closeAsk = askPrice;
                  lastFrameBar.closeMid = midPrice;

                  if (lastFrameBar.closeBid <= lastFrameBar.lowBid) lastFrameBar.lowBid = lastFrameBar.closeBid;
                  if (lastFrameBar.closeAsk <= lastFrameBar.lowAsk) lastFrameBar.lowAsk = lastFrameBar.closeAsk;
                  if (lastFrameBar.closeMid <= lastFrameBar.lowMid) lastFrameBar.lowMid = lastFrameBar.closeMid;

                  if (lastFrameBar.closeBid >= lastFrameBar.highBid) lastFrameBar.highBid = lastFrameBar.closeBid;
                  if (lastFrameBar.closeAsk >= lastFrameBar.highAsk) lastFrameBar.highAsk = lastFrameBar.closeAsk;
                  if (lastFrameBar.closeMid >= lastFrameBar.highMid) lastFrameBar.highMid = lastFrameBar.closeMid;
               }
               else
               {
                  lastFrameBar.complete = true;

                  PriceBar newBar = new PriceBar()
                  {
                     time = MAOE.Utilities.GetTimeAsXmlSerializedUtc(instrumentChart.CreateNewFrameTime.Value),

                     openBid = bidPrice,
                     highBid = bidPrice,
                     lowBid = bidPrice,
                     closeBid = bidPrice,

                     openAsk = askPrice,
                     highAsk = askPrice,
                     lowAsk = askPrice,
                     closeAsk = askPrice,

                     openMid = midPrice,
                     highMid = midPrice,
                     lowMid = midPrice,
                     closeMid = midPrice,

                     volume = 1
                  };

                  instrumentChart.Frames.Add(CreateChartFrame(newBar, instrumentChart));
               }
            }

            MAOE.Utilities.ReleaseTick();
         }
      }

      protected virtual void OnConnectionStatusChanged(ApiConnectionStatus status, string type)
      {
         if (status == ApiConnectionStatus.Connected)
            AddAlgorithmMessage("OANDA Api connection status is: Connected.", true, TraceEventType.Information);
         else if (status == ApiConnectionStatus.Streaming)
            AddAlgorithmMessage(string.Format("OANDA Api connection status is: Streaming {0}.", type), true, TraceEventType.Information);
         else if (status == ApiConnectionStatus.Faulted)
            AddAlgorithmMessage("OANDA Api connection status is: Faulted.", true, TraceEventType.Information);
         else if (status == ApiConnectionStatus.Recovered)
            AddAlgorithmMessage("OANDA Api connection status is: Recovered.", true, TraceEventType.Information);
         else if (status == ApiConnectionStatus.Disconnected)
            AddAlgorithmMessage("OANDA Api connection status is: Disconnected.", true, TraceEventType.Information);

         if (_instance.Status == (short?)AlgorithmStatus.Running)
         {
            if (status == ApiConnectionStatus.Connected)
            {
               // api connection restored
               if (_lastConnectionStatus == ApiConnectionStatus.Recovered)
               {
                  // get missed account activity
                  UpdateStrategyTransactions().Wait();
                  UpdateOrdersAndTrades().Wait();
               }
            }
         }

         OnConnectionStatusChangedAction(status);

         _lastConnectionStatus = status;
      }

      protected abstract void OnConnectionStatusChangedAction(ApiConnectionStatus status);

      protected virtual void OnConnectionRecoveryMessaged(string message)
      {
         AddAlgorithmMessage(message, true, TraceEventType.Information);
      }
      #endregion

      #region utilities
      protected async Task<bool> UpdateOrdersAndTrades()
      {
         AddAlgorithmMessage("Updating instance orders and trades ...", false, TraceEventType.Information);

         short orderCount = 0;
         short tradeCount = 0;

         MCE.Signal[] signals = await SubscriptionCaller.Instance().GetActiveSignalsByTypeAsync(MACC.Constants.SignalType.Thrust);

         foreach (MCE.Signal signal in signals)
         {
            if (signal.StrategyTransactionID.HasValue)
            {
               IEnumerable<StrategyTransaction> transactions;
               StrategyTransaction orderTransaction, tradeTransaction, exitTransaction;

               // get transaction collection from db
               int orderTransactionID = signal.StrategyTransactionID.Value;
               transactions = StrategyCaller.Instance().GetStrategyTransactionsCollectionAsync(orderTransactionID).Result;
               transactions = transactions.Where(t => t.StrategyID == _strategy.StrategyID);
               transactions = transactions.OrderBy(t => t.BrokerTransactionID);

               orderTransaction = transactions.FirstOrDefault(t => t.StrategyTransactionID == orderTransactionID);
               tradeTransaction = transactions.FirstOrDefault(t => t.Type == MACC.Constants.TransactionTypes.OrderFilled);
               exitTransaction = transactions.FirstOrDefault(t => t.BrokerTradeID != null && t.Type != MACC.Constants.TransactionTypes.TradeUpdate);

               if (exitTransaction != null)
               {
                  signal.Active = false;
                  SubscriptionCaller.Instance().UpdateSignalAsync(signal);

                  continue;
               }

               if (tradeTransaction != null)
               {
                  tradeCount++;

                  long tradeId = Convert.ToInt64(tradeTransaction.BrokerTransactionID);

                  TradeData oandaTrade = await Rest.GetTradeDetailsAsync(_accountId, tradeId);

                  MarketMinerTrade mmTrade = new MarketMinerTrade(oandaTrade)
                  {
                     SignalID = signal.SignalID,
                     StrategyTransactionID = signal.StrategyTransactionID,
                     Closed = exitTransaction != null
                  };

                  AddOrUpdateAlgorithmTrade(mmTrade);

                  continue;
               }

               if (orderTransaction != null)
               {
                  orderCount++;

                  long orderId = Convert.ToInt64(orderTransaction.BrokerTransactionID);

                  Order oandaOrder = await Rest.GetOrderDetailsAsync(_accountId, orderId);

                  MarketMinerOrder mmOrder = new MarketMinerOrder(oandaOrder)
                  {
                     SignalID = signal.SignalID,
                     StrategyTransactionID = signal.StrategyTransactionID,
                     Filled = tradeTransaction != null,
                     Cancelled = tradeTransaction != null
                  };

                  AddOrUpdateAlgorithmOrder(mmOrder);
               }
            }
         }

         AddAlgorithmMessage(string.Format("{0} open orders updated.", orderCount), false, TraceEventType.Information);

         AddAlgorithmMessage(string.Format("{0} open trades updated.", tradeCount), false, TraceEventType.Information);

         return true;
      }
      #endregion
   }
}
