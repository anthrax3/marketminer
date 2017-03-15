using MarketMiner.Client.Entities;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace MarketMiner.Client.Contracts
{
   [ServiceContract]
   public interface IStrategyService : IServiceContract
   {
      #region Operations
      [OperationContract]
      void PostAlgorithmStatusNotification(string message);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      Strategy[] GetStrategies();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      Strategy GetStrategy(int strategyId);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      Strategy UpdateStrategy(Strategy strategy);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      AlgorithmInstance[] GetAlgorithmInstances();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      AlgorithmParameter[] GetAlgorithmParameters(int strategyId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      AlgorithmInstance[] GetAlgorithmInstancesByStrategy(int strategyId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      AlgorithmInstance[] GetAlgorithmInstancesByStatus(short status);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      AlgorithmInstance UpdateAlgorithmInstance(AlgorithmInstance instance);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      AlgorithmMessage UpdateAlgorithmMessage(AlgorithmMessage message);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      StrategyTransaction GetStrategyTransaction(int transactionId);

      [OperationContract]
      StrategyTransaction[] GetStrategyTransactions(int? count, bool? descending);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      StrategyTransaction[] GetStrategyTransactionsCollection(int entryStrategyTransactionId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      StrategyTransaction[] GetStrategyTransactionsByDateRange(DateTime dateBottom, DateTime? dateTop, int maxTransactions);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      StrategyTransaction SaveStrategyTransaction(StrategyTransaction transaction, string brokerName);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      StrategyTransaction UpdateStrategyTransaction(StrategyTransaction transaction);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      Broker[] GetBrokers();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      Broker GetBroker(int brokerId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      Broker GetBrokerByName(string name);
      #endregion

      #region Operations.Async
      [OperationContract]
      Task PostAlgorithmStatusNotificationAsync(string message);

      [OperationContract]
      Task<Strategy[]> GetStrategiesAsync();

      [OperationContract]
      Task<Strategy> GetStrategyAsync(int strategyId);

      [OperationContract]
      Task<Strategy> UpdateStrategyAsync(Strategy strategy);

      [OperationContract]
      Task<AlgorithmParameter[]> GetAlgorithmParametersAsync(int strategyId);

      [OperationContract]
      Task<AlgorithmInstance[]> GetAlgorithmInstancesAsync();

      [OperationContract]
      Task<AlgorithmInstance[]> GetAlgorithmInstancesByStrategyAsync(int strategyId);

      [OperationContract]
      Task<AlgorithmInstance[]> GetAlgorithmInstancesByStatusAsync(short status);

      [OperationContract]
      Task<AlgorithmInstance> UpdateAlgorithmInstanceAsync(AlgorithmInstance instance);

      [OperationContract]
      Task<AlgorithmMessage> UpdateAlgorithmMessageAsync(AlgorithmMessage message);

      [OperationContract]
      Task<StrategyTransaction> GetStrategyTransactionAsync(int transactionId);

      [OperationContract]
      Task<StrategyTransaction[]> GetStrategyTransactionsAsync(int? count, bool? descending);

      [OperationContract]
      Task<StrategyTransaction[]> GetStrategyTransactionsCollectionAsync(int entryStrategyTransactionId);

      [OperationContract]
      Task<StrategyTransaction[]> GetStrategyTransactionsByDateRangeAsync(DateTime dateBottom, DateTime? dateTop, int maxTransactions);

      [OperationContract]
      Task<StrategyTransaction> UpdateStrategyTransactionAsync(StrategyTransaction transaction);

      [OperationContract]
      Task<StrategyTransaction> SaveStrategyTransactionAsync(StrategyTransaction transaction, string brokerName);

      [OperationContract]
      Task<Broker[]> GetBrokersAsync();

      [OperationContract]
      Task<Broker> GetBrokerAsync(int brokerId);

      [OperationContract]
      Task<Broker> GetBrokerByNameAsync(string name);
      #endregion
   }
}