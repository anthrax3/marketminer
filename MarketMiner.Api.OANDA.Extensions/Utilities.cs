using MarketMiner.Api.OANDA.Extensions.Classes;
using MarketMiner.Api.OANDA.REST.TradeLibrary;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes;
using MarketMiner.Api.OANDA.REST.TradeLibrary.DataTypes.Communications.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace MarketMiner.Api.OANDA.Extensions
{
   public static class Utilities
   {
      #region Rates stream methods
      static bool _rateStreamStarted;
      static Semaphore _tickReceived;
      static MarketMinerRatesSession _ratesSession;

      public static bool RatesStreamStarted { get { return _rateStreamStarted; } }

      public static async Task StartRatesStreamAsync(EEnvironment environment, string token, int account, List<string> instruments, StreamSession<RateStreamResponse>.DataHandler dataHandler, RatesSession.SessionStatusHandler statusHandler)
      {
         if (Credentials.GetDefaultCredentials() == null)
            Credentials.SetCredentials(environment, token, account);

         if (!_rateStreamStarted)
         {
            var oandaInstruments = await Rest.GetInstrumentsAsync(account, null, instruments);
            _ratesSession = new MarketMinerRatesSession(account, oandaInstruments);

            if (dataHandler != null) 
               _ratesSession.DataReceived += dataHandler;

            // register fault handlers .. yes, the order matters
            _ratesSession.SessionStatusChanged += (s, e) => _rateStreamStarted = false;
            if (statusHandler != null)
               _ratesSession.SessionStatusChanged += statusHandler;

            _tickReceived = new Semaphore(0, 100);
            _ratesSession.StartSession();

            if (!_ratesSession.Stopped())
            {
               // good start
               _tickReceived.WaitOne(10000);
               _rateStreamStarted = true;
            }
         }
         else
         {
            _ratesSession.DataReceived += dataHandler;
         }
      }

      public static void ReleaseTick()
      {
         _tickReceived.Release();
      }

      public static void StopRatesStream()
      {
         _ratesSession.StopSession();
         _rateStreamStarted = false;
      }
      #endregion

      #region Events stream methods
      static bool _eventsStreamStarted;
      static Semaphore _eventReceived;
      static MarketMinerEventsSession _eventsSession;

      public static bool EventsStreamStarted { get { return _eventsStreamStarted; } }

      public static async Task StartEventsStreamAsync(EEnvironment environment, string token, int account, StreamSession<Event>.DataHandler dataHandler, EventsSession.SessionStatusHandler faultHandler)
      {
         if (Credentials.GetDefaultCredentials() == null)
            Credentials.SetCredentials(environment, token, account);

         if (!_eventsStreamStarted)
         {
            _eventsSession = new MarketMinerEventsSession(account);

            if (dataHandler != null) 
               _eventsSession.DataReceived += dataHandler;

            // register fault handlers .. yes, the order matters
            _eventsSession.SessionStatusChanged += (s, e) => _eventsStreamStarted = false;
            if (faultHandler != null)
               _eventsSession.SessionStatusChanged += faultHandler;

            _eventReceived = new Semaphore(0, 100);
            _eventsSession.StartSession();

            if (!_eventsSession.Stopped())
            {
               // good start
               _eventReceived.WaitOne(10000);
               _eventsStreamStarted = true;
            }
         }
         else
         {
            _eventsSession.DataReceived += dataHandler;
         }
      }

      public static void ReleaseEvent()
      {
         _eventReceived.Release();
      }

      public static void StopEventsStream()
      {
         _eventsSession.StopSession();
         _eventsStreamStarted = false;
      }
      #endregion

      #region Utilities
      public static async Task<List<Candle>> GetCandlesAsync(string instrument, EGranularity granularity, int count)
      {
         Func<CandlesRequest> request = () => new CandlesRequest
         {
            instrument = instrument,
            granularity = granularity,
            count = count
         };

         List<Candle> pollCandles = null;

         pollCandles = await Rest.GetCandlesAsync(request());

         return pollCandles;
      }

      public static void RegisterDataStreamHandler<T>(StreamSession<T>.DataHandler handler) where T: IHeartbeat
      {
         if (typeof(T) != typeof(Event) && typeof(T) != typeof(RateStreamResponse))
            throw new ArgumentException(string.Format("Unrecognized stream data handler type: ", typeof(T)));

         if (_eventsStreamStarted && typeof(T) == typeof(Event))
            _eventsSession.DataReceived += handler as StreamSession<Event>.DataHandler;
         else if (_rateStreamStarted && typeof(T) == typeof(RateStreamResponse))
            _ratesSession.DataReceived += handler as StreamSession<RateStreamResponse>.DataHandler;
         else
            throw new InvalidOperationException(string.Format("Stream not started for handler type: ", typeof(T)));         
      }

      public static string GetTimeAsXmlSerializedUtc(DateTime time)
      {
         string xmlUtcTime = XmlConvert.ToString(time, "yyyy-MM-ddTHH:mm:ssZ");

         return xmlUtcTime;
      }
      #endregion
   }
}
