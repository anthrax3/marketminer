using MarketMiner.Algorithm.Common;
using MarketMiner.Algorithm.Common.Contracts;
using MarketMiner.Api.Client.OANDA.Common;
using MarketMiner.Api.OANDA;
using MarketMiner.Api.Tests.Custom;
using MarketMiner.Client.Bootstrapper;
using N.Core.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MarketMiner.Algorithms
{
   class MarketMiner
   {
      static void Main(string[] args)
      {
         InitializeContainer();

         try
         {
            bool runTests = false;
            bool okToStart = true;
            bool ignoreTests = true;

            Console.WriteLine("\nStarting MarketMiner.Algorithms ...");

            #region tests .. move to unit tests?
            if (runTests)
            {
               Console.WriteLine("\nTests running. Please wait ...");

               var tests = new CustomOANDARestTest();

               // hook up the prop change handler
               tests.Results.PropertyChanged += ((s, e) =>
               {
                  //Console.WriteLine(((RestTestResults)s).LastMessage);
               });

               // run
               var task = tests.RunTest(EEnvironment.Practice, Security.Token.PracticeToken, Security.Account.PracticeAccount);
               task.Wait();

               Console.WriteLine(string.Format("\nTests complete. There were {0} failures.", tests.Failures));

               // run starts?
               okToStart = tests.Failures.Equals(0);
               if (!okToStart)
               {
                  short tickErrors = 0;
                  foreach (var message in tests.Results.Messages.Values)
                  {
                     if (message.IndexOf("False").Equals(0) && message.ToLower().Contains("streaming tick"))
                        tickErrors++;
                  }

                  okToStart = tickErrors == 0;
               }
            }
            #endregion

            if (okToStart || ignoreTests)
               AsyncPump.Run(() => StartAlgorithms());

            Console.WriteLine("\nMarketMiner.Algorithms is started. Please press enter to Exit");
            Console.ReadLine();
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nMarketMiner.Algorithms has faulted. Please press enter to Exit");
            Console.WriteLine("\nERROR: " + ex.Message);
            Console.WriteLine("\nPlease press enter to Exit.");

            Console.ReadLine();
         }
      }

      static List<IAlgorithm> _algorithms = new List<IAlgorithm>();

      static async void StartAlgorithms()
      {
         Console.WriteLine("\nStarting algorithm modules ...");

         _algorithms = ObjectBase.Container.GetExportedValues<IAlgorithm>().ToList();

         foreach (var algorithm in _algorithms)
         {
            if (algorithm.Instance == null)
               Task.Factory.StartNew(() => StartAlgorithm(algorithm));
         }

         Console.WriteLine("\nAlgorithm modules started.");
      }

      static async Task StartAlgorithm(IAlgorithm algorithm)
      {
         IEnumerable<Attribute> attributes = algorithm.GetType().GetCustomAttributes(typeof(AlgorithmModuleAttribute));

         if (attributes.Count() > 0)
         {
            AlgorithmModuleAttribute attribute = attributes.FirstOrDefault() as AlgorithmModuleAttribute;
            algorithm.Logger.LogToConsole = true;
            if (await algorithm.Initialize(attribute.StrategyID)) 
               await algorithm.Start();
         }
         else
         {
            // log it?
         }
      }

      static FileSystemWatcher _watcher;
      static void InitializeContainer()
      {
         DirectoryCatalog moduleCatalog = new DirectoryCatalog(ConfigurationManager.AppSettings["moduleFolder"], "*.dll");

         /// monitor the Modules folder for dlls
         _watcher = new FileSystemWatcher(ConfigurationManager.AppSettings["moduleFolder"], "*.dll");
         _watcher.Created += (sender, e) => { moduleCatalog.Refresh(); };
         _watcher.Changed += (sender, e) => { moduleCatalog.Refresh(); };
         _watcher.EnableRaisingEvents = true;

         // load Mef container
         ObjectBase.Container = MEFLoader.Init(new List<ComposablePartCatalog>()
         {
            moduleCatalog
            , new AssemblyCatalog(Assembly.GetExecutingAssembly())
            , new AssemblyCatalog(typeof(AlgorithmBase).Assembly)
            //, new DirectoryCatalog(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath))
         });

         // start the strategy algorithm if one was added
         ObjectBase.Container.ExportsChanged += (sender, e) =>
         {
            foreach (var item in e.AddedExports)
            {
               if (item.ContractName == "MarketMiner.Algorithm.Common.Contracts.IAlgorithm")
               {
                  Console.WriteLine(string.Format("\nNew {0} snap-in detected.", item.ContractName));

                  //ObjectBase.Container.ComposeExportedValue(item);
                  IAlgorithm algorithm = item as IAlgorithm;
                  _algorithms.Add(algorithm);

                  AsyncPump.Run(() => Task.Factory.StartNew(() => StartAlgorithm(algorithm)));
                  //Task.Factory.StartNew(() => StartAlgorithm(algorithm));
               }
            }
         };
      }
   }
}
