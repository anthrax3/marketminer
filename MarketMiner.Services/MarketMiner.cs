using MarketMiner.Business.Bootstrapper;
using MarketMiner.Business.Managers;
using Microsoft.AspNet.SignalR.Client;
using N.Core.Common.Contracts;
using N.Core.Common.Core;
using System;
using SM = System.ServiceModel;

namespace MarketMiner.Services
{
   class MarketMiner
   {
      #region Declarations
      static SM.ServiceHost _hostAccountManager;
      static SM.ServiceHost _hostMetadataManager;
      static SM.ServiceHost _hostParticipationManager;
      static SM.ServiceHost _hostSubscriptionManager;
      static SM.ServiceHost _hostStrategyManager;

      static HubConnection _connection;
      static IHubProxy _mmHub;
      static IEventLogger _logger;
      #endregion

      static void Main(string[] args)
      {
         try
         {
            //GenericPrincipal principal = new GenericPrincipal(
            //      new GenericIdentity("Osita"), new string[] { OCOApp.Security.User, OCOApp.Security.Admin });
            //Thread.CurrentPrincipal = principal;

            Console.WriteLine("\nInitializing components ...");
            ObjectBase.Container = MEFLoader.Init();  // DI: repositories and business engines

            _logger = ObjectBase.Container.GetExportedValue<IEventLogger>();
            _logger.LogToConsole = true;
            Console.WriteLine("\nComponents initialized.");
            //

            Console.WriteLine("\nBootstrapping database ...");
            MarketMinerContextBootstrapper.Init(); // initializes the MarketMinerContext
            Console.WriteLine("\nDatabase ready.");
            //

            //ConnectToSignalR();
            StartServices();     
            //

            Console.WriteLine("\nMarketMiner services are started and running.");
         }
         catch (Exception ex)
         {
            string message = "\nMarketMiner.Services experienced an unhandled exception.";

            if (_logger != null)
               _logger.LogException(ex, message);
            else
               Console.WriteLine("\nERROR: " + ex.Message);
         }
         finally
         {
            Console.WriteLine("\nPlease press [Enter] to Exit");
            Console.ReadLine();

            // wcf
            StopService(_hostAccountManager, "AccountManager");
            StopService(_hostMetadataManager, "MetadataManager");
            StopService(_hostParticipationManager, "ParticipationManager");
            StopService(_hostSubscriptionManager, "SubscriptionManager");
            StopService(_hostStrategyManager, "StrategyManager");
            Console.WriteLine("\nServices stopped.");

            // signalr
            //_connection.Stop();
            Console.WriteLine("\nSignalR service stopped.\n");
         }
      }

      static void StartServices()
      {
         //GenericPrincipal principal = new GenericPrincipal(
         //      new GenericIdentity("Osita"), new string[] { Security.User, Security.Admin });
         //Thread.CurrentPrincipal = principal;

         Console.WriteLine("\nStarting Wcf services ...");

         _hostAccountManager = new SM.ServiceHost(typeof(AccountManager));
         _hostMetadataManager = new SM.ServiceHost(typeof(MetadataManager));
         _hostParticipationManager = new SM.ServiceHost(typeof(ParticipationManager));
         _hostSubscriptionManager = new SM.ServiceHost(typeof(SubscriptionManager));
         _hostStrategyManager = new SM.ServiceHost(typeof(StrategyManager));

         StartService(_hostAccountManager, "AccountManager");
         StartService(_hostMetadataManager, "MetadataManager");
         StartService(_hostParticipationManager, "ParticipationManager");
         StartService(_hostSubscriptionManager, "SubscriptionManager");
         StartService(_hostStrategyManager, "StrategyManager");

         Console.WriteLine("\nWcf services started.");
      }

      /// <summary>
      /// Starts the MarketMiner tcp services
      /// </summary>
      /// <param name="host"></param>
      /// <param name="serviceDescription"></param>
      static void StartService(SM.ServiceHost host, string serviceDescription)
      {
         host.Open();
         Console.WriteLine("\nService {0} started.", serviceDescription);

         foreach (var endpoint in host.Description.Endpoints)
         {
            Console.WriteLine(string.Format(" -- Address: {0}", endpoint.Address.Uri.ToString()));
            Console.WriteLine(string.Format(" -- Binding: {0}", endpoint.Binding.Name));
            Console.WriteLine(string.Format(" -- Contract: {0}\n", endpoint.Contract.ConfigurationName));
         }
      }

      /// <summary>
      /// Stops the AutoRental tcp services
      /// </summary>
      /// <param name="host"></param>
      /// <param name="serviceDescription"></param>
      static void StopService(SM.ServiceHost host, string serviceDescription)
      {
         if (host == null)
            return;

         if (host.State != SM.CommunicationState.Closed)
         {
            host.Close();
            Console.WriteLine("\nService {0} stopped.", serviceDescription);
         }
      }

      static void ConnectToSignalR()
      {
         Console.WriteLine("\nConnecting to SignalR service ...");

         // create connection and proxy
         _connection = new HubConnection("http://localhost:8080/signalr");
         _mmHub = _connection.CreateHubProxy("MarketMinerHub");

         //start connection
         _connection.Start().ContinueWith(task =>
         {
            if (task.IsFaulted)
            {
               Console.WriteLine("\nThere was an error opening the connection:{0}", task.Exception.GetBaseException());
            }
            else
            {
               Console.WriteLine("\nConnected to MarketMiner SignalR server.");
            }
         }).Wait();

         // say hello
         _mmHub.Invoke<string>("SendAll", "MarketMiner.Services", "HELLO World. This is MarketMiner.Services.").ContinueWith(task =>
         {
            if (task.IsFaulted)
            {
               Console.WriteLine("\nThere was an error calling SendAll: {0}", task.Exception.GetBaseException());
            }
            else
            {
               Console.WriteLine(task.Result);
            }
         });

         RegisterHubMessageHandlers();

         _mmHub.Invoke<string>("SendAll", "MarketMiner.Console is doing something!!!").Wait();

         Console.WriteLine("\nConnected to SignalR service ...\n");
      }

      static void RegisterHubMessageHandlers() 
      {
         // register received message handlers
         _mmHub.On<string, string>("addMessage", (sender, message) =>
         {
            Console.WriteLine(sender + ": " + message);
         });
      }
   }
}
