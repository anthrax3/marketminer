using MarketMiner.Client.Contracts;
using MarketMiner.Client.Entities;
using N.Core.Common.ServiceModel;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace MarketMiner.Client.Proxies
{
   [Export(typeof(IStrategyService))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class StrategyClient : UserClientBase<IStrategyService>, IStrategyService
   {
      #region Operations
      public void PostAlgorithmStatusNotification(string message)
      {
         Channel.PostAlgorithmStatusNotification(message);
      }

      public Strategy[] GetStrategies()
      {
         return Channel.GetStrategies();
      }

      public Strategy GetStrategy(int strategyId)
      {
         return Channel.GetStrategy(strategyId);
      }

      public Strategy UpdateStrategy(Strategy strategy)
      {
         return Channel.UpdateStrategy(strategy);
      }

      public AlgorithmParameter[] GetAlgorithmParameters(int strategyId)
      {
         return Channel.GetAlgorithmParameters(strategyId);
      }

      public AlgorithmInstance[] GetAlgorithmInstances()
      {
         return Channel.GetAlgorithmInstances();
      }

      public AlgorithmInstance[] GetAlgorithmInstancesByStrategy(int strategyId)
      {
         return Channel.GetAlgorithmInstancesByStrategy(strategyId);
      }

      public AlgorithmInstance[] GetAlgorithmInstancesByStatus(short status)
      {
         return Channel.GetAlgorithmInstancesByStatus(status);
      }

      public AlgorithmInstance UpdateAlgorithmInstance(AlgorithmInstance instance)
      {
         return Channel.UpdateAlgorithmInstance(instance);
      }

      public AlgorithmMessage UpdateAlgorithmMessage(AlgorithmMessage message)
      {
         return Channel.UpdateAlgorithmMessage(message);
      }

      public StrategyTransaction GetStrategyTransaction(int transactionId)
      {
         return Channel.GetStrategyTransaction(transactionId);
      }

      public StrategyTransaction[] GetStrategyTransactions(int? count, bool? descending)
      {
         return Channel.GetStrategyTransactions(count, descending);
      }

      public StrategyTransaction[] GetStrategyTransactionsCollection(int entryStrategyTransactionId)
      {
         return Channel.GetStrategyTransactionsCollection(entryStrategyTransactionId);
      }

      public StrategyTransaction[] GetStrategyTransactionsByDateRange(DateTime dateBottom, DateTime? dateTop, int maxTransactions)
      {
         return Channel.GetStrategyTransactionsByDateRange(dateBottom, dateTop, maxTransactions);
      }

      public StrategyTransaction UpdateStrategyTransaction(StrategyTransaction transaction)
      {
         return Channel.UpdateStrategyTransaction(transaction);
      }

      public StrategyTransaction SaveStrategyTransaction(StrategyTransaction transaction, string brokerName)
      {
         return Channel.SaveStrategyTransaction(transaction, brokerName);
      }

      public Broker[] GetBrokers()
      {
         return Channel.GetBrokers();
      }

      public Broker GetBroker(int brokerId)
      {
         return Channel.GetBroker(brokerId);
      }

      public Broker GetBrokerByName(string name)
      {
         return Channel.GetBrokerByName(name);
      }
      #endregion

      #region Operations.Async
      public Task PostAlgorithmStatusNotificationAsync(string message)
      {
         return Channel.PostAlgorithmStatusNotificationAsync(message);
      }

      public Task<Strategy[]> GetStrategiesAsync()
      {
         return Channel.GetStrategiesAsync();
      }

      public Task<Strategy> GetStrategyAsync(int strategyId)
      {
         return Channel.GetStrategyAsync(strategyId);
      }

      public Task<Strategy> UpdateStrategyAsync(Strategy strategy)
      {
         return Channel.UpdateStrategyAsync(strategy);
      }

      public Task<AlgorithmParameter[]> GetAlgorithmParametersAsync(int strategyId)
      {
         return Channel.GetAlgorithmParametersAsync(strategyId);
      }

      public Task<AlgorithmInstance[]> GetAlgorithmInstancesAsync()
      {
         return Channel.GetAlgorithmInstancesAsync();
      }

      public Task<AlgorithmInstance[]> GetAlgorithmInstancesByStrategyAsync(int strategyId)
      {
         return Channel.GetAlgorithmInstancesByStrategyAsync(strategyId);
      }

      public Task<AlgorithmInstance[]> GetAlgorithmInstancesByStatusAsync(short status)
      {
         return Channel.GetAlgorithmInstancesByStatusAsync(status);
      }

      public Task<AlgorithmInstance> UpdateAlgorithmInstanceAsync(AlgorithmInstance instance)
      {
         return Channel.UpdateAlgorithmInstanceAsync(instance);
      }

      public Task<AlgorithmMessage> UpdateAlgorithmMessageAsync(AlgorithmMessage message)
      {
         return Channel.UpdateAlgorithmMessageAsync(message);
      }

      public Task<StrategyTransaction> GetStrategyTransactionAsync(int transactionId)
      {
         return Channel.GetStrategyTransactionAsync(transactionId);
      }

      public Task<StrategyTransaction[]> GetStrategyTransactionsAsync(int? count, bool? descending)
      {
         return Channel.GetStrategyTransactionsAsync(count, descending);
      }

      public Task<StrategyTransaction[]> GetStrategyTransactionsCollectionAsync(int entryStrategyTransactionId)
      {
         return Channel.GetStrategyTransactionsCollectionAsync(entryStrategyTransactionId);
      }

      public Task<StrategyTransaction[]> GetStrategyTransactionsByDateRangeAsync(DateTime dateBottom, DateTime? dateTop, int maxTransactions)
      {
         return Channel.GetStrategyTransactionsByDateRangeAsync(dateBottom, dateTop, maxTransactions);
      }

      public Task<StrategyTransaction> UpdateStrategyTransactionAsync(StrategyTransaction transaction)
      {
         return Channel.UpdateStrategyTransactionAsync(transaction);
      }

      public Task<StrategyTransaction> SaveStrategyTransactionAsync(StrategyTransaction transaction, string brokerName)
      {
         return Channel.SaveStrategyTransactionAsync(transaction, brokerName);
      }

      public Task<Broker[]> GetBrokersAsync()
      {
         return Channel.GetBrokersAsync();
      }

      public Task<Broker> GetBrokerAsync(int brokerId)
      {
         return Channel.GetBrokerAsync(brokerId);
      }

      public Task<Broker> GetBrokerByNameAsync(string name)
      {
         return Channel.GetBrokerByNameAsync(name);
      }
      #endregion
   }
}
