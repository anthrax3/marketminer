using MarketMiner.Algorithm.Common.Contracts;
using MarketMiner.Api.Client.Common;
using MarketMiner.Api.Client.Common.Charting;
using MarketMiner.Api.Common.Contracts;
using MarketMiner.Client.Entities;
using MarketMiner.Client.Proxies.ServiceCallers;
using MarketMiner.Common.Enums;
using N.Core.Common.Contracts;
using N.Core.Common.Core;
using P.Core.Common.Contracts;
using P.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMiner.Algorithm.Common
{
   public abstract class AlgorithmBase : IAlgorithm
   {
      public AlgorithmBase(IEventLogger logger)
      {
         _logger = logger;
         _logger.LogToConsole = true;
      }

      #region Declarations
      IEventLogger _logger;
      protected bool _shutdown = false;
      protected Func<bool> _suspended;
      protected int _accountId;
      protected Strategy _strategy;
      protected AlgorithmInstance _instance;
      protected ObservableCollection<AlgorithmMessage> _messages;
      protected string _shortClassName;
      #endregion

      #region Properties
      public AlgorithmParameters Parameters { get; private set; }
      #endregion

      #region Members.IAlgorithm
      public ILogger Logger { get { return _logger; } }
      public Strategy Strategy { get { return _strategy; } }
      public AlgorithmInstance Instance { get { return _instance; } }
      public ObservableCollection<AlgorithmMessage> Messages { get { return _messages; } }

      public virtual async Task<bool> Initialize(int strategyId)
      {
         try
         {
            AddAlgorithmMessage(string.Format("{0} is starting ...", _shortClassName), true, TraceEventType.Start);

            if (strategyId <= 0)
               throw new ArgumentException("StrategyId may not be less than or equal to 0.");

            if (_logger == null)
               _logger = ObjectBase.Container.GetExportedValue<IEventLogger>();

            MetadataCaller.Instance().ClearCache();

            Strategy strategy = StrategyCaller.Instance().GetStrategy(strategyId);
            if (strategy == null)
               throw new ArgumentException(string.Format("Strategy not found for strategyId {0}.", strategyId));

            _strategy = strategy;
            _shortClassName = _strategy.Name.Split('.').ToList().Last();

            Parameters = new AlgorithmParameters(await StrategyCaller.Instance().GetAlgorithmParametersAsync(_strategy.StrategyID));

            // create and save instance
            _instance = new AlgorithmInstance() { StrategyID = strategyId, Status = (short?)AlgorithmStatus.NotRunning };
            await UpdateAlgorithmInstance();

            _messages = new ObservableCollection<AlgorithmMessage>();

            // hook up event handlers
            Messages.CollectionChanged += OnAlgorithmMessageChanged;

            _suspended = () => { return _instance.Status == (short?)AlgorithmStatus.RunningSuspended; };

            AddAlgorithmMessage(string.Format("{0} is initialized.", _shortClassName), true, TraceEventType.Start);
         }
         catch (Exception e)
         {
            Exception ex = new Exception(string.Format("Algorithm for strategyId: {0} failed to initialize.", strategyId), e);
            AddAlgorithmException(ex).Wait();
            return false;
         }

         return true;
      }

      public virtual async Task<bool> Start()
      {
         UpdateAlgorithmInstanceProperty("Status", (short?)AlgorithmStatus.Starting);
         UpdateAlgorithmInstanceProperty("RunStartDateTime", DateTime.UtcNow);

         return true;
      }

      public virtual async Task<bool> Stop()
      {
         _shutdown = true;

         UpdateAlgorithmInstanceProperty("Status", (short?)AlgorithmStatus.NotRunning);
         UpdateAlgorithmInstanceProperty("RunStopDateTime", DateTime.UtcNow);

         AddAlgorithmMessage(string.Format("{0} instance stopped.", _shortClassName), true, TraceEventType.Stop);

         return _shutdown;
      }
      #endregion

      #region utilities
      private async Task UpdateAlgorithmInstance()
      {
         _instance = await StrategyCaller.Instance().UpdateAlgorithmInstanceAsync(_instance);
      }

      protected virtual async Task UpdateAlgorithmInstanceProperty(string propertyName, object value, string message = null)
      {
         _instance.SetValue(propertyName, value);

         UpdateAlgorithmInstance();

         Action<string> addAlgorithmMessage = (msg) => AddAlgorithmMessage(string.Format("{0} {1}", _shortClassName, msg, true, TraceEventType.Information));

         // write message to db and to subscribers
         switch (propertyName)
         {
            case "Status":
               addAlgorithmMessage(string.Format("status changed to: {0}", _instance.Status.AsAlgorithmStatusString()));
               break;
            case "RunStartDateTime":
               addAlgorithmMessage(string.Format("run start date-time is: {0}", _instance.RunStartDateTime.ToString()));
               break;
            case "RunStopDateTime":
               addAlgorithmMessage(string.Format("run stop date-time is: {0}", _instance.RunStopDateTime.ToString()));
               break;
            default:
               addAlgorithmMessage(message ?? string.Format("property value changed."));
               break;
         }
      }

      protected async Task AddAlgorithmMessage(string message, bool saveMesage = true, TraceEventType type = 0, Exception ex = null)
      {
         var newMessage = new AlgorithmMessage()
         {
            AlgorithmInstanceID = _instance.AlgorithmInstanceID,
            Message = message,
            Severity = type,
            Exception = ex
         };

         // limit coll. size due to memory
         if (Messages.Count >= 200) Messages.RemoveAt(0);
         Messages.Add(newMessage);

         if (saveMesage)
            StrategyCaller.Instance().UpdateAlgorithmMessageAsync(newMessage);
      }

      protected async Task AddAlgorithmException(Exception ex, string message = null)
      {
         AddAlgorithmMessage(message ?? ex.Message, true, TraceEventType.Error, ex);
      }
      #endregion

      #region event handlers
      private void OnAlgorithmMessageChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
         if (e.Action == NotifyCollectionChangedAction.Add)
         {
            foreach (var item in e.NewItems)
            {
               AlgorithmMessage message = (AlgorithmMessage)item;

               if (_logger != null)
               {
                  if (message.Exception != null)
                     _logger.LogException(message.Severity, message.Exception, message.Message);
                  else
                     _logger.LogMessage(message.Severity, message.Message);
               }
            }
         }
         if (e.Action == NotifyCollectionChangedAction.Replace)
         {
            //
         }
         if (e.Action == NotifyCollectionChangedAction.Remove)
         {
            //
         }
         if (e.Action == NotifyCollectionChangedAction.Move)
         {
            //
         }
      }
      #endregion
   }
}
