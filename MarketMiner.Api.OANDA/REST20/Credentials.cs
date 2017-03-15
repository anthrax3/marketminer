using System.Collections.Generic;

namespace MarketMiner.Api.OANDA.REST20
{
   public enum EServer
   {
      Account,
      Rates,
      StreamingRates,
      StreamingEvents,
      Labs
   }

   public enum EEnvironment
   {
      Sandbox,
      Practice,
      Trade
   }

   public class Credentials20
   {
      public bool HasServer(EServer server)
      {
         return Servers[Environment].ContainsKey(server);
      }

      public string GetServer(EServer server)
      {
         if (HasServer(server))
         {
            return Servers[Environment][server];
         }
         return null;
      }

      private static readonly Dictionary<EEnvironment, Dictionary<EServer, string>> Servers = new Dictionary<EEnvironment, Dictionary<EServer, string>>
         {
            {EEnvironment.Sandbox, new Dictionary<EServer, string>
               {
                  {EServer.Account, "http://api-sandbox.oanda.com/v3/"},
                  {EServer.Rates, "http://api-sandbox.oanda.com/v3/"},
                  {EServer.StreamingRates, "http://stream-sandbox.oanda.com/v3/"},
                  {EServer.StreamingEvents, "http://stream-sandbox.oanda.com/v3/"},
               }
            },
            {EEnvironment.Practice, new Dictionary<EServer, string>
               {
                  {EServer.StreamingRates, "https://stream-fxpractice.oanda.com/v3/"},
                  {EServer.StreamingEvents, "https://stream-fxpractice.oanda.com/v3/"},
                  {EServer.Account, "https://api-fxpractice.oanda.com/v3/"},
                  {EServer.Rates, "https://api-fxpractice.oanda.com/v3/"},
                  {EServer.Labs, "https://api-fxpractice.oanda.com/labs/v3/"},
               }
            },
            {EEnvironment.Trade, new Dictionary<EServer, string>
               {
                  {EServer.StreamingRates, "https://stream-fxtrade.oanda.com/v3/"},
                  {EServer.StreamingEvents, "https://stream-fxtrade.oanda.com/v3/"},
                  {EServer.Account, "https://api-fxtrade.oanda.com/v3/"},
                  {EServer.Rates, "https://api-fxtrade.oanda.com/v3/"},
                  {EServer.Labs, "https://api-fxtrade.oanda.com/labs/v3/"},
               }
            }
         };

      public string AccessToken;

      private static Credentials20 _instance;
      public string DefaultAccountId;
      public EEnvironment Environment;

      public bool IsSandbox
      {
         get { return Environment == EEnvironment.Sandbox; }
      }
      public string Username;

      public static Credentials20 GetDefaultCredentials()
      {
         if (_instance == null)
         {
            //_instance = GetPracticeCredentials();
            //_instance = GetSandboxCredentials();
         }
         return _instance;
      }

      private static Credentials20 GetSandboxCredentials()
      {
         return new Credentials20()
            {
               Environment = EEnvironment.Sandbox,
            };
      }

      private static Credentials20 GetPracticeCredentials()
      {
         return new Credentials20()
            {
               DefaultAccountId = "621396",
               Environment = EEnvironment.Practice,
               AccessToken = "73eba38ad5b44778f9a0c0fec1a66ed1-44f47f052c897b3e1e7f24196bbc071f"
            };
         
      }

      private static Credentials20 GetLiveCredentials()
      {
         // You'll need to add your own accessToken and account if desired
         return new Credentials20()
            {
               //defaultAccountId = 00000,
               //accessToken = "fhaishihfweaiuu2u892h829h829h92ha8rfa89",
               Environment = EEnvironment.Trade
            };
      }

      public static void SetCredentials(EEnvironment environment, string accessToken, string defaultAccount = "0")
      {
         _instance = new Credentials20
         {
            Environment = environment,
            AccessToken = accessToken,
            DefaultAccountId = defaultAccount
         };
      }
   }
}
