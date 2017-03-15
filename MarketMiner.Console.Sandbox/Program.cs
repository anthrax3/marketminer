using MarketMiner.Algorithm.Demo;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace MarketMiner.Console.Sandbox
{
   class Program
   {
      //static void Main(string[] args)
      //{
      //    Task perdiodicTask = PeriodicTaskFactory.Start(() =>
      //    {
      //        System.Console.WriteLine(DateTime.Now);
      //    }, intervalInMilliseconds: 2000, // fire every two seconds...
      //       maxIterations: 10);           // for a total of 10 iterations...

      //    perdiodicTask.ContinueWith(_ =>
      //    {
      //        System.Console.WriteLine("Finished!");
      //    }).Wait();
      //}

      static void Main(string[] args)
      {
         var state = CommunicationHandler.ExecuteMethod("GetMarketState", "", "", "CurrencyExchangeHub");
         System.Console.WriteLine("Market State is " + state);

         if (state == "Closed")
         {
            var returnCode = CommunicationHandler.ExecuteMethod("OpenMarket", "", "", "CurrencyExchangeHub");
            Debug.Assert(returnCode == "True");
            System.Console.WriteLine("Market State is Open");
         }

         System.Console.ReadLine();
      }
   }

   
   public static class CommunicationHandler
   {
      static IHubProxy _currencyExchangeHub = null;

      public static string ExecuteMethod(string method, string args, string serverUri, string hubName)
      {
         if (_currencyExchangeHub == null)
         {
            var hubConnection = new HubConnection("http://localhost:8080");
            _currencyExchangeHub = hubConnection.CreateHubProxy("CurrencyExchangeHub");

            // register client method(s)
            _currencyExchangeHub.On<Currency>("NotifyChange", HandleNotifyChange);

            // Start the connection
            hubConnection.Start().Wait();
         }

         var result = _currencyExchangeHub.Invoke<string>(method).Result;

         return result;
      }

      private static void HandleNotifyChange(Currency currency)
      {
         System.Console.WriteLine("Currency " + currency.CurrencySign + ", Rate = " + currency.USDValue);
      }
   }
}