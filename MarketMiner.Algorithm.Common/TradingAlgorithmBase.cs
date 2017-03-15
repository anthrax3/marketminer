using MarketMiner.Algorithm.Common.Contracts;
using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Common.Contracts;
using MarketMiner.Client.Entities;
using MarketMiner.Client.Proxies.ServiceCallers;
using N.Core.Common.Contracts;
using P.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMiner.Algorithm.Common
{
   public abstract class TradingAlgorithmBase : AlgorithmBase
   {
      #region Declarations
      protected ApiConnectionStatus _lastConnectionStatus = ApiConnectionStatus.Disconnected;
      protected int _chartInitialFrames;
      protected int _chartMaximumFrames;
      protected int _chartRefreshMilliSeconds;
      protected bool _chartsInitialized;
      protected Dictionary<string, Chart> _charts;
      protected ObservableCollection<IOrder> _orders;
      protected ObservableCollection<ITrade> _trades;
      protected ITakeProfitPriceCalculator _takeProfitPriceCalculator;
      protected IStopLossPriceCalculator _stopLossPriceCalculator;
      #endregion

      #region Properties
      public Dictionary<string, Chart> Charts { get { return _charts; } }
      public ObservableCollection<IOrder> Orders { get { return _orders; } }
      public ObservableCollection<ITrade> Trades { get { return _trades; } }
      #endregion

      public TradingAlgorithmBase(IEventLogger logger)
         : base(logger)
      {
      }

      public override async Task<bool> Initialize(int strategyId)
      {
         bool result = await base.Initialize(strategyId);

         _chartInitialFrames = Parameters.GetInteger("chartInitialFrames") ?? 200;
         _chartMaximumFrames = Parameters.GetInteger("chartMaximumFrames") ?? 500;
         _chartRefreshMilliSeconds = Parameters.GetInteger("chartRefreshMilliSeconds") ?? 60000;

         return result;
      }

      public override async Task<bool> Start()
      {
         bool result = await base.Start();

         _charts = new Dictionary<string, Chart>();
         _orders = new ObservableCollection<IOrder>();
         _trades = new ObservableCollection<ITrade>();

         return result;
      }

      #region charts
      /// <summary>
      /// Creates a new chart frame from an IPriceBar and adds it to the chart
      /// </summary>
      /// <param name="bar">An IPriceBar object.</param>
      /// <param name="chart">A Chart object.</param>
      /// <returns>The new Frame object that was added to the chart.</returns>
      protected virtual Frame CreateChartFrame(IPriceBar bar, Chart chart)
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
      #endregion

      #region orders and trades
      public void AddOrUpdateAlgorithmOrder(IOrder order)
      {
         IOrder algorithmOrder = _orders.FirstOrDefault(o => o.id == order.id);

         if (algorithmOrder == null)
            _orders.Add(order);
         else
            algorithmOrder.InjectWith(order);
      }

      public void AddOrUpdateAlgorithmTrade(ITrade trade)
      {
         ITrade algorithmTrade = _trades.FirstOrDefault(t => t.id == trade.id);

         if (algorithmTrade == null)
            _trades.Add(trade);
         else
            algorithmTrade.InjectWith(trade);
      }
      #endregion

      #region Utilities
      protected virtual async Task AddStrategyTransaction(StrategyTransaction transaction)
      {
         StrategyTransaction updatedTransaction = await StrategyCaller.Instance().UpdateStrategyTransactionAsync(transaction);

         _strategy.Transactions.Add(updatedTransaction);

         string message = "New Transaction: " + updatedTransaction.Side + " : " + updatedTransaction.Instrument;

         await AddAlgorithmMessage(message, true, TraceEventType.Information);
      }

      protected override async Task UpdateAlgorithmInstanceProperty(string propertyName, object value, string message = null)
      {
         string tradingMessage = null;

         Func<string, string> buildMessage = msg => { return string.Format("{0} {1}", _shortClassName, msg); };

         switch (propertyName)
         {
            case "FirstTradeDateTime":
               tradingMessage = buildMessage(string.Format("first trade date-time is: {0}", _instance.FirstTradeDateTime.ToString()));
               break;
            case "LastTradeDateTime":
               tradingMessage = buildMessage(string.Format("last trade date-time is: {0}", _instance.LastTradeDateTime.ToString()));
               break;
            default:
               break;
         }

         await base.UpdateAlgorithmInstanceProperty(propertyName, value, tradingMessage);
      }

      protected virtual async Task PostAlgorithmStatusNotification(string name)
      {
         string message = string.Format("{0} : {1} : {2}", name, _instance.AlgorithmInstanceID, Enum.GetName(typeof(ApiConnectionStatus), _lastConnectionStatus));

         StrategyCaller.Instance().PostAlgorithmStatusNotificationAsync(message);
      }
      #endregion
   }
}
