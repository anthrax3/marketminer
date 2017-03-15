using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Client.OANDA.Common;
using MarketMiner.Api.Client.OANDA.Data.DataModels;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.OANDA.REST.TradeLibrary;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications.Requests;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAOE = MarketMiner.Api.OANDA.Extensions;

namespace MarketMiner.Api.Client.OANDA.Data
{
   public class DataManager
   {
      #region Declarations
      private static Dictionary<int, AccountData> accounts = new Dictionary<int, AccountData>();
      private static ApiConnectionStatus _apiConnectionStatus = ApiConnectionStatus.Disconnected;
      #endregion

      #region Properties
      public static DataManager Instance { get { return new DataManager(); } }
      public static ApiConnectionStatus ApiConnectionStatus { get { return _apiConnectionStatus; } }
      #endregion

      public static AccountData GetAccountData(int id)
      {
         if (!accounts.ContainsKey(id))
         {
            accounts.Add(id, new AccountData(id));
         }
         return accounts[id];
      }

      private static async Task RecoverApiConnection(string reason, Exception e)
      {
         bool connectionRecovered = false;

         OnConnectionRecovering(string.Format("Fault Error: {0}", reason));
         if (e != null)
            OnConnectionRecovering(string.Format("Exception Error: {0}", e.Message));

         await Task.Run(() =>
         {
            short count = 1;

            while (!connectionRecovered)
            {
               string message = "";

               if (_apiConnectionStatus == ApiConnectionStatus.Faulted
                  || _apiConnectionStatus == ApiConnectionStatus.Recovering)
               {
                  _apiConnectionStatus = ApiConnectionStatus.Recovering;

                  if (count > 1)
                     OnConnectionRecovering("Event stream re-start attempt failed.");

                  Task.Delay(TimeSpan.FromMilliseconds(5000)).Wait();

                  message = string.Format("OANDA connection recovery attempt: {0}", count.ToString());
                  OnConnectionRecovering(message);

                  message = StopEventsStream();
                  OnConnectionRecovering(message);

                  // 5 secs
                  Task.Delay(TimeSpan.FromMilliseconds(5000)).Wait();
                  OnConnectionRecovering("Attempting to re-start events stream ...");

                  StartEventsStreamAsync();

                  // 25 secs .. to allow time for a hearbeat
                  // http://developer.oanda.com/rest-live/streaming/#eventsStreaming
                  Task.Delay(TimeSpan.FromMilliseconds(25000)).Wait();

                  count++;
               }

               if (_apiConnectionStatus == ApiConnectionStatus.Recovered)
               {
                  connectionRecovered = true;

                  OnConnectionRecovering("OANDA connection recovered.");

                  OnConnectionStatusChanged(ApiConnectionStatus.Recovered, null);

                  Task.Delay(TimeSpan.FromMilliseconds(1500)).Wait();

                  OnConnectionStatusChanged(ApiConnectionStatus.Connected, null);
               }
            }
         });
      }

      public async Task<List<PriceBar>> GetPriceBars(List<Tuple<string, EGranularity>> barSpecsList, int count, ECandleFormat priceFormat = ECandleFormat.midpoint)
      {
         List<PriceBar> bars = null;

         if (_apiConnectionStatus == ApiConnectionStatus.Connected || _apiConnectionStatus == ApiConnectionStatus.Streaming)
         {
            try
            {
               bars = await RatesDataSource.Instance.GetPriceBars(barSpecsList, count);
            }
            catch (Exception e)
            {
               OnConnectionStatusChanged(ApiConnectionStatus.Faulted, null);

               // try to recover
               RecoverApiConnection("DataManager did not receive bars from RatesDataSource.", null);

               return null;
            }
         }

         return bars;
      }

      #region rate streaming
      public static async Task StartRatesStreamAsync(List<string> instruments)
      {
         Credentials credentials = Credentials.GetDefaultCredentials();

         if (credentials == null)
            throw new InvalidOperationException("OANDA credentials not set. Please use StartEventsStreamAsync(....) instead. Make sure to supply the needed credentials.");

         await StartRatesStreamAsync(credentials.Environment, credentials.AccessToken, credentials.DefaultAccountId, instruments, null);
      }

      public static async Task StartRatesStreamAsync(List<string> instruments, StreamSession<RateStreamResponse>.DataHandler onRateDataReceived)
      {
         await StartRatesStreamAsync(EEnvironment.Practice, Security.Token.PracticeToken, Security.Account.PracticeAccount, instruments, onRateDataReceived);
      }

      public static async Task StartRatesStreamAsync(EEnvironment environment, string token, int account, List<string> instruments, StreamSession<RateStreamResponse>.DataHandler handler)
      {
         await MAOE.Utilities.StartRatesStreamAsync(environment, token, account, instruments, handler, OnRatesSessionStatusChanged);
      }

      public static string StopRatesStream()
      {
         MAOE.Utilities.StopRatesStream();
         return "OANDA rates stream stopped.";
      }
      #endregion

      #region event streaming
      public static async Task StartEventsStreamAsync()
      {
         Credentials credentials = Credentials.GetDefaultCredentials();

         if (credentials == null)
            throw new InvalidOperationException("OANDA credentials not set. Please use StartEventsStreamAsync(....) instead. Make sure to supply the needed credentials.");

         await StartEventsStreamAsync(credentials.Environment, credentials.AccessToken, credentials.DefaultAccountId, null);
      }

      public static async Task StartEventsStreamAsync(StreamSession<Event>.DataHandler onEventDataReceived)
      {
         Credentials credentials = Credentials.GetDefaultCredentials();
         await StartEventsStreamAsync(credentials.Environment, credentials.AccessToken, credentials.DefaultAccountId, onEventDataReceived);
      }

      public static async Task StartEventsStreamAsync(EEnvironment environment, string token, int account, StreamSession<Event>.DataHandler handler)
      {
         await MAOE.Utilities.StartEventsStreamAsync(environment, token, account, handler, OnEventsSessionStatusChanged);
      }

      public static string StopEventsStream()
      {
         MAOE.Utilities.StopEventsStream();
         return "OANDA events stream stopped.";
      }
      #endregion

      #region events
      public static string RegisterDataStreamHandler<T>(StreamSession<T>.DataHandler handler) where T : IHeartbeat
      {
         MAOE.Utilities.RegisterDataStreamHandler<T>(handler);

         return string.Format("OANDA {0} stream handler registered.", typeof(T).Name.ToString());
      }

      public delegate void ConnectionStatusHandler(MarketMiner.Api.Client.Common.ApiConnectionStatus status, string type);
      public static event ConnectionStatusHandler ConnectionStatusChanged;
      public static void OnConnectionStatusChanged(ApiConnectionStatus status, string type)
      {
         _apiConnectionStatus = status;

         ConnectionStatusHandler handler = ConnectionStatusChanged;
         if (handler != null) handler(status, type);
      }

      public delegate void ConnectionRecoveryHandler(string message);
      public static event ConnectionRecoveryHandler ConnectionRecoveryMessaged;
      public static void OnConnectionRecovering(string message)
      {
         ConnectionRecoveryHandler handler = ConnectionRecoveryMessaged;
         if (handler != null) handler(message);
      }

      public static void OnEventsSessionStatusChanged(bool started, Exception e)
      {
         if (e != null)
         {
            if (_apiConnectionStatus != ApiConnectionStatus.Recovering)
            {
               OnConnectionStatusChanged(ApiConnectionStatus.Faulted, null);
               RecoverApiConnection(e.Message, e.InnerException);
            }
         }
         else if (_apiConnectionStatus == ApiConnectionStatus.Recovering && started)
            _apiConnectionStatus = ApiConnectionStatus.Recovered;
         else if (_apiConnectionStatus == ApiConnectionStatus.Disconnected && started)
            OnConnectionStatusChanged(ApiConnectionStatus.Streaming, "Events");
      }

      public static void OnRatesSessionStatusChanged(bool started, Exception e)
      {
         if (e != null)
         {
            if (_apiConnectionStatus != ApiConnectionStatus.Recovering)
            {
               OnConnectionStatusChanged(ApiConnectionStatus.Faulted, null);
               RecoverApiConnection(e.Message, e.InnerException);
            }
            //StartRatesStreamAsync();
         }
         else if (_apiConnectionStatus == ApiConnectionStatus.Disconnected && started)
            OnConnectionStatusChanged(ApiConnectionStatus.Streaming, "Rates");
      }
      #endregion
   }
}
