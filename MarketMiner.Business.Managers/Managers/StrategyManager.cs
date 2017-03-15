using MarketMiner.Business.Contracts;
using MarketMiner.Business.Entities;
using MarketMiner.Common;
using MarketMiner.Data.Contracts;
using N.Core.Business.Contracts;
using N.Core.Common.ServiceModel;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;

namespace MarketMiner.Business.Managers
{
   [ApplyProxyDataContractResolver]
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, ReleaseServiceInstanceOnTransactionComplete = false)]
   public class StrategyManager : ManagerBase, IStrategyService
   {
      #region Constructors
      public StrategyManager() { }

      public StrategyManager(IDataRepositoryFactory dataRepositoryFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
      }

      public StrategyManager(IBusinessEngineFactory businessEngineFactory)
      {
         _BusinessEngineFactory = businessEngineFactory;
      }

      public StrategyManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
         _BusinessEngineFactory = businessEngineFactory;
      }
      #endregion

      #region Operations.IStrategyService
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public void PostAlgorithmStatusNotification(string message)
      {
         ExecuteFaultHandledOperation(() =>
         {
            IStrategyEngine strategyEngine = _BusinessEngineFactory.GetBusinessEngine<IStrategyEngine>();

            strategyEngine.PostAlgorithmStatusNotification(message);
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public Strategy[] GetStrategies()
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IStrategyRepository strategyRepository = _DataRepositoryFactory.GetDataRepository<IStrategyRepository>();
            IEnumerable<Strategy> strategies = strategyRepository.Get();

            return strategies.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      public Strategy GetStrategy(int strategyId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            Strategy strategy = GetStrategies().Where(s => s.StrategyID == strategyId).FirstOrDefault();

            if (strategy == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No strategy found for id '{0}'.", strategyId));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }
            return strategy;
         });
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Strategy UpdateStrategy(Strategy strategy)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IStrategyRepository strategyRepository = _DataRepositoryFactory.GetDataRepository<IStrategyRepository>();

            Strategy updatedEntity = null;

            if (strategy.StrategyID == 0)
               updatedEntity = strategyRepository.Add(strategy);
            else
               updatedEntity = strategyRepository.Update(strategy);

            return updatedEntity;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public AlgorithmInstance[] GetAlgorithmInstances()
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IAlgorithmInstanceRepository instanceRepository = _DataRepositoryFactory.GetDataRepository<IAlgorithmInstanceRepository>();
            IEnumerable<AlgorithmInstance> instances = instanceRepository.Get();

            return instances.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public AlgorithmParameter[] GetAlgorithmParameters(int strategyId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            // does it exist?
            GetStrategy(strategyId);

            IAlgorithmParameterRepository parameterRepository = _DataRepositoryFactory.GetDataRepository<IAlgorithmParameterRepository>();
            IEnumerable<AlgorithmParameter> parameters = parameterRepository.GetAlgorithmParametersByStrategy(strategyId);

            return parameters.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public AlgorithmInstance[] GetAlgorithmInstancesByStrategy(int strategyId)
      {
         return GetAlgorithmInstances().Where(i => i.StrategyID == strategyId).ToArray();
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public AlgorithmInstance[] GetAlgorithmInstancesByStatus(short status)
      {
         return GetAlgorithmInstances().Where(i => i.Status == status).ToArray();
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public AlgorithmInstance UpdateAlgorithmInstance(AlgorithmInstance instance)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            AlgorithmInstance updatedEntity = null;
            Strategy strategy = GetStrategy(instance.StrategyID);

            if (strategy != null)
            {
               IAlgorithmInstanceRepository algorithmInstanceRepository = _DataRepositoryFactory.GetDataRepository<IAlgorithmInstanceRepository>();

               if (instance.AlgorithmInstanceID == 0)
                  updatedEntity = algorithmInstanceRepository.Add(instance);
               else
                  updatedEntity = algorithmInstanceRepository.Update(instance);
            }
            return updatedEntity;
         });
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public AlgorithmMessage UpdateAlgorithmMessage(AlgorithmMessage message)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IAlgorithmInstanceRepository algorithmInstanceRepository = _DataRepositoryFactory.GetDataRepository<IAlgorithmInstanceRepository>();
            AlgorithmInstance instance = algorithmInstanceRepository.Get(message.AlgorithmInstanceID);

            if (instance == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No algorithm instance found for instanceId '{0}'.", message.AlgorithmInstanceID));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            IAlgorithmMessageRepository messageRepository = _DataRepositoryFactory.GetDataRepository<IAlgorithmMessageRepository>();
            AlgorithmMessage updatedEntity = null;

            if (message.AlgorithmMessageID == 0)
               updatedEntity = messageRepository.Add(message);
            else
               updatedEntity = messageRepository.Update(message);

            return updatedEntity;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public StrategyTransaction GetStrategyTransaction(int transactionId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IStrategyTransactionRepository transactionRepository = _DataRepositoryFactory.GetDataRepository<IStrategyTransactionRepository>();

            StrategyTransaction transaction = transactionRepository.Get(transactionId);

            if (transaction == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No transaction found with id '{0}'.", transactionId));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            return transaction;
         });
      }

      /// <summary>
      /// Fetches and returns specified count of strategy transactions.
      /// </summary>
      /// <param name="count">The number of transactions to return up to a maximum of 5000.</param>
      /// <param name="descending">The direction of the strategy transactions read.</param>
      /// <returns></returns>
      public StrategyTransaction[] GetStrategyTransactions(int? count, bool? descending)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            count = Math.Min(5000, count ?? 5000);

            descending = descending ?? false;

            IStrategyTransactionRepository transactionRepository = _DataRepositoryFactory.GetDataRepository<IStrategyTransactionRepository>();

            IEnumerable<StrategyTransaction> transactions = null;

            if (descending.Value)
               transactions = transactionRepository.Get().OrderByDescending(t => t.BrokerTransactionID).Take(count.Value);
            else
               transactions = transactionRepository.Get().OrderBy(t => t.BrokerTransactionID).Take(count.Value);

            return transactions.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public StrategyTransaction[] GetStrategyTransactionsCollection(int entryStrategyTransactionId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            // get the entry order transaction
            StrategyTransaction transaction = GetStrategyTransaction(entryStrategyTransactionId);

            // add it to the return collection
            List<StrategyTransaction> transactions = new List<StrategyTransaction>() { transaction };

            IStrategyTransactionRepository transactionRepository = _DataRepositoryFactory.GetDataRepository<IStrategyTransactionRepository>();

            List<string> searchIds = new List<string>() { transaction.BrokerTransactionID };

            // find the related transactions
            while (searchIds.Count > 0)
            {
               string searchId = searchIds.First();

               searchIds.RemoveAt(0);

               List<StrategyTransaction> relatedTransactions = transactionRepository.Get().Where(t => t.BrokerOrderID == searchId || t.BrokerTradeID == searchId).ToList();

               // grab their BrokerTransactionId's
               relatedTransactions.ForEach(t => searchIds.Add(t.BrokerTransactionID));

               // add them to the collection
               transactions.AddRange(relatedTransactions);
            }

            return transactions.ToArray();
         });
      }

      /// <summary>
      /// Fetches and returns all strategy transaction entries (inclusive) in the specified range.
      /// </summary>
      /// <param name="dateBottom">The earliest transaction time.</param>
      /// <param name="dateTop">The latest transaction time.</param>
      /// <param name="maxTransactions">The number of transactions to return up to a maximum of 5000.</param>
      /// <returns></returns>
      public StrategyTransaction[] GetStrategyTransactionsByDateRange(DateTime dateBottom, DateTime? dateTop, int maxTransactions)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            maxTransactions = Math.Min(5000, maxTransactions);

            dateTop = dateTop ?? DateTime.MaxValue.ToUniversalTime();

            if (dateBottom >= dateTop)
               throw new ArgumentOutOfRangeException(string.Format("dateBottom [{0}] is greater than dateTop [{1}].", dateBottom, dateTop));
            if (dateTop <= dateBottom)
               throw new ArgumentOutOfRangeException(string.Format("dateTop [{0}] is less than dateBottom [{1}].", dateTop, dateBottom));

            IStrategyTransactionRepository transactionRepository = _DataRepositoryFactory.GetDataRepository<IStrategyTransactionRepository>();

            StrategyTransaction[] transactions = transactionRepository.GetStrategyTransactionsByDateRange(dateBottom, dateTop.Value).Take(maxTransactions).ToArray();

            return transactions;
         });
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public StrategyTransaction SaveStrategyTransaction(StrategyTransaction transaction, string brokerName)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            StrategyTransaction updatedEntity = null;
            Strategy strategy = GetStrategy(transaction.StrategyID);

            if (strategy != null)
            {
               IStrategyTransactionRepository transactionRepository = _DataRepositoryFactory.GetDataRepository<IStrategyTransactionRepository>();

               if (transaction.StrategyTransactionID == 0)
               {
                  transaction.BrokerID = GetBrokerByName(brokerName).BrokerID;
                  updatedEntity = transactionRepository.Add(transaction);
               }
               else
                  updatedEntity = transactionRepository.Update(transaction);
            }
            return updatedEntity;
         });
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public StrategyTransaction UpdateStrategyTransaction(StrategyTransaction transaction)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            string brokerName = GetBroker(transaction.BrokerID).Name;

            StrategyTransaction updatedEntity = SaveStrategyTransaction(transaction, brokerName);

            return updatedEntity;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Broker GetBrokerByName(string name)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            Broker broker = GetBrokers().Where(b => b.Name == name).FirstOrDefault();

            if (broker == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No broker found with name '{0}'.", name));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            return broker;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Broker GetBroker(int brokerId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            Broker broker = GetBrokers().Where(b => b.BrokerID == brokerId).FirstOrDefault();

            if (broker == null)
            {
               NotFoundFault fault = new NotFoundFault(string.Format("No broker found with BrokeID '{0}'.", brokerId));
               throw new FaultException<NotFoundFault>(fault, fault.Message);
            }

            return broker;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Broker[] GetBrokers()
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IBrokerRepository brokerRepository = _DataRepositoryFactory.GetDataRepository<IBrokerRepository>();

            Broker[] brokers = brokerRepository.Get().ToArray();

            return brokers;
         });
      }
      #endregion
   }
}
