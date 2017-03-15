using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using ServiceProcess.Helpers;
using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace MarketMiner.Host.SignalR
{
   static class Program
   {
      private static readonly List<ServiceBase> _servicesToRun = new List<ServiceBase>();

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      static void Main()
      {
         var mmSignalR = MarketMinerSignalR.Instance;
         var url = mmSignalR.ServiceUrl;

         _servicesToRun.Add(mmSignalR);

         if (Environment.UserInteractive)
         {
            System.Console.WriteLine("Server running on {0}", url);
            //System.Console.ReadLine();
            _servicesToRun.LoadServices();
         }
         else
         {
            ServiceBase.Run(_servicesToRun.ToArray());
         }         
      }
   }

   public class Startup
   {
      public void Configuration(IAppBuilder app)
      {
         app.Map("/signalr", map =>
         {
            // Setup the cors middleware to run before SignalR.
            // By default this will allow all origins. You can 
            // configure the set of origins and/or http verbs by
            // providing a cors options with a different policy.
            map.UseCors(CorsOptions.AllowAll);

            var hubConfiguration = new HubConfiguration
            {
               // You can enable JSONP by uncommenting line below.
               // JSONP requests are insecure but some older browsers (and some
               // versions of IE) require JSONP to work cross domain
               EnableJSONP = true,
               EnableDetailedErrors = true
            };

            // Run the SignalR pipeline. We're not using MapSignalR
            // since this branch is already runs under the "/signalr"
            // path.
            map.RunSignalR(hubConfiguration);
         });
      }
   }
}
