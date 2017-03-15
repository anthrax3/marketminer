using MarketMiner.Algorithm.Demo;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketMiner.Host.SignalR
{
   public partial class MarketMinerSignalR : ServiceBase
   {
      #region Windows Service

      #region Declarations/properties
      private string _url = "http://localhost:8080";
      private Thread _mainThread;
      private bool _isRunning = true;
      private Random _random = new Random();
      IDisposable _webApp = null;

      public string ServiceUrl { get { return _url; } }
      //

      private readonly static Lazy<MarketMinerSignalR> _instance = new Lazy<MarketMinerSignalR>(
         () => new MarketMinerSignalR(GlobalHost.ConnectionManager.GetHubContext<CurrencyExchangeHub>().Clients));
      public static MarketMinerSignalR Instance { get { return _instance.Value; } }
      #endregion

      #region
      public MarketMinerSignalR(IHubConnectionContext<dynamic> clients)
      {
         InitializeComponent();
         this.ServiceName = "MarketMinerSignalR";
         CurrencyExchangeClients = clients;
      }
      #endregion

      protected override void OnStart(string[] args)
      {
         _webApp = WebApp.Start(_url); // Must be @"http://+:8080" if you want to connect from other computers
         LoadDefaultCurrencies();

         // Start main thread .. invoke RunService(object timeToStop)
         _mainThread = new Thread(new ParameterizedThreadStart(this.RunService));
         _mainThread.Start(DateTime.UtcNow.AddMinutes(5));
      }

      protected override void OnStop()
      {
         _mainThread.Join();
         _webApp.Dispose();
      }

      public void RunService(object timeToStop)
      {
         DateTime stopTime = timeToStop != null ? Convert.ToDateTime(timeToStop) : DateTime.MaxValue;

         while (_isRunning && DateTime.UtcNow < stopTime)
         {
            Thread.Sleep(15000); // 15 seconds
            NotifyAllClients();
         }
      }

      private void NotifyAllClients()
      {
         Currency currency = new Currency() { CurrencySign = "CAD", USDValue = _random.Next() };
         //BroadcastCurrencyRate(currency);
         CurrencyExchangeClients.All.NotifyChange(currency);
      }
      #endregion
      //----------------------------------------------------------

      #region MarketMinerSignalR operations

      #region Declarations
      private Timer _timer;
      private volatile bool _updatingCurrencyRates;
      private volatile MarketState _marketState;
      private readonly double _rangePercent = 0.002;
      private readonly object _marketStateLock = new object();
      private readonly object _updateCurrencyRatesLock = new object();
      private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
      private readonly ConcurrentDictionary<string, Currency> _currencies = new ConcurrentDictionary<string, Currency>();
      private readonly Random _updateOrNotRandom = new Random();

      private IHubConnectionContext<dynamic> CurrencyExchangeClients { get; set; }
      #endregion

      #region Public - CurrencyExchangeHub delegates
      public MarketState MarketState { get { return _marketState; } private set { _marketState = value; } }
      public TimeSpan UpdateInterval { get { return _updateInterval; } }

      public IEnumerable<Currency> GetAllCurrencies()
      {
         return _currencies.Values;
      }

      public bool OpenMarket()
      {
         bool returnCode = false;

         lock (_marketStateLock)
         {
            if (MarketState != MarketState.Open)
            {
               _timer = new Timer(UpdateCurrencyRates, null, _updateInterval, _updateInterval);
               MarketState = MarketState.Open;
               BroadcastMarketStateChange(MarketState.Open);
            }
         }
         returnCode = true;

         return returnCode;
      }

      public bool CloseMarket()
      {
         bool returnCode = false;

         lock (_marketStateLock)
         {
            if (MarketState == MarketState.Open)
            {
               if (_timer != null)
               {
                  _timer.Dispose();
               }

               MarketState = MarketState.Closed;
               BroadcastMarketStateChange(MarketState.Closed);
            }
         }
         returnCode = true;

         return returnCode;
      }

      public bool Reset()
      {
         bool returnCode = false;

         lock (_marketStateLock)
         {
            if (MarketState != MarketState.Closed)
            {
               throw new InvalidOperationException("Market must be closed before it can be reset.");
            }

            LoadDefaultCurrencies();
            BroadcastMarketReset();
         }
         returnCode = true;

         return returnCode;
      }
      #endregion

      #region Private - CurrencyExchangeHub client broadcasts
      private void BroadcastMarketStateChange(MarketState marketState)
      {
         switch (marketState)
         {
            case MarketState.Open:
               CurrencyExchangeClients.All.marketOpened();
               break;
            case MarketState.Closed:
               CurrencyExchangeClients.All.marketClosed();
               break;
            default:
               break;
         }
      }

      private void BroadcastMarketReset()
      {
         CurrencyExchangeClients.All.marketReset();
      }

      private void BroadcastCurrencyRate(Currency currency)
      {
         CurrencyExchangeClients.All.updateCurrencyRate(currency);
      }
      #endregion

      #region Private - Utility methods
      private void LoadDefaultCurrencies()
      {
         _currencies.Clear();

         var currencies = new List<Currency>
            {
                new Currency { CurrencySign = "USD", USDValue = 1.00m },
                new Currency { CurrencySign = "CAD", USDValue = 0.85m },
                new Currency { CurrencySign = "EUR", USDValue = 1.25m }
            };

         currencies.ForEach(currency => _currencies.TryAdd(currency.CurrencySign, currency));
      }

      private void UpdateCurrencyRates(object state)
      {
         // This function must be re-entrant as it's running as a timer interval handler
         lock (_updateCurrencyRatesLock)
         {
            if (!_updatingCurrencyRates)
            {
               _updatingCurrencyRates = true;

               foreach (var currency in _currencies.Values)
               {
                  if (TryUpdateCurrencyRate(currency))
                  {
                     BroadcastCurrencyRate(currency);
                  }
               }

               _updatingCurrencyRates = false;
            }
         }
      }

      private bool TryUpdateCurrencyRate(Currency currency)
      {
         // Randomly choose whether to update this currency or not
         var r = _updateOrNotRandom.NextDouble();
         if (r > 0.1)
         {
            return false;
         }

         // Update the currency price by a random factor of the range percent
         var random = new Random((int)Math.Floor(currency.USDValue));
         var percentChange = random.NextDouble() * _rangePercent;
         var pos = random.NextDouble() > 0.51;
         var change = Math.Round(currency.USDValue * (decimal)percentChange, 2);
         change = pos ? change : -change;

         currency.USDValue += change;
         return true;
      }
      #endregion

      #endregion
   }
     
   public enum MarketState
   {
      Closed,
      Open
   }
}
