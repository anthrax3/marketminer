using MarketMiner.Client.Contracts;
using MarketMiner.Client.Entities;
using System;
using System.Threading.Tasks;

namespace MarketMiner.Client.Proxies.ServiceCallers
{
   public class StrategyCaller : ServiceCaller
   {
      public static StrategyCaller Instance() { return new StrategyCaller(); }

      #region Operation result types
      Strategy _strategyResult;
      AlgorithmInstance _algorithmInstanceResult;
      AlgorithmMessage _algorithmMessageResult;
      StrategyTransaction _transactionResult;

      AlgorithmParameter[] _parameterResults;
      StrategyTransaction[] _transactionResults;
      #endregion

      #region Operations
      public void PostAlgorithmStatusNotification(string message)
      {
         WithStrategyClient(strategyClient =>
         {
            strategyClient.PostAlgorithmStatusNotification(message);
         });
      }

      public AlgorithmParameter[] GetAlgorithmParameters(int strategyId)
      {
         WithStrategyClient(strategyClient =>
         {
            _parameterResults = strategyClient.GetAlgorithmParameters(strategyId);
         });
         return _parameterResults;
      }

      public Strategy GetStrategy(int strategyId)
      {
         WithStrategyClient(strategyClient =>
         {
            _strategyResult = strategyClient.GetStrategy(strategyId);
         });
         return _strategyResult;
      }

      public AlgorithmInstance UpdateAlgorithmInstance(AlgorithmInstance instance)
      {
         WithStrategyClient(strategyClient =>
         {
            _algorithmInstanceResult = strategyClient.UpdateAlgorithmInstance(instance);
         });
         return _algorithmInstanceResult;
      }

      public AlgorithmMessage UpdateAlgorithmMessage(AlgorithmMessage message)
      {
         WithStrategyClient(strategyClient =>
          {
             _algorithmMessageResult = strategyClient.UpdateAlgorithmMessage(message);
          });
         return _algorithmMessageResult;
      }

      public StrategyTransaction GetStrategyTransaction(int transactionId)
      {
         WithStrategyClient(strategyClient =>
         {
            _transactionResult = strategyClient.GetStrategyTransaction(transactionId);
         });
         return _transactionResult;
      }

      public StrategyTransaction SaveStrategyTransaction(StrategyTransaction transaction, string brokerName)
      {
         WithStrategyClient(strategyClient =>
         {
            _transactionResult = strategyClient.SaveStrategyTransaction(transaction, brokerName);
         });
         return _transactionResult;
      }

      public StrategyTransaction UpdateStrategyTransaction(StrategyTransaction transaction)
      {
         WithStrategyClient(strategyClient =>
         {
            _transactionResult = strategyClient.UpdateStrategyTransaction(transaction);
         });
         return _transactionResult;
      }

      public StrategyTransaction[] GetStrategyTransactions(int? count, bool? descending)
      {
         WithStrategyClient(strategyClient =>
         {
            _transactionResults = strategyClient.GetStrategyTransactions(count, descending);
         });
         return _transactionResults;
      }

      public StrategyTransaction[] GetStrategyTransactionsCollection(int entryStrategyTransactionId)
      {
         WithStrategyClient(strategyClient =>
         {
            _transactionResults = strategyClient.GetStrategyTransactionsCollection(entryStrategyTransactionId);
         });
         return _transactionResults;
      }

      public StrategyTransaction[] GetStrategyTransactionsByDateRange(DateTime dateBottom, DateTime? dateTop, int maxTransactions)
      {
         WithStrategyClient(strategyClient =>
         {
            _transactionResults = strategyClient.GetStrategyTransactionsByDateRange(dateBottom, dateTop, maxTransactions);
         });
         return _transactionResults;
      }
      #endregion

      #region Operations.Async
      public async Task PostAlgorithmStatusNotificationAsync(string message)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            await strategyClient.PostAlgorithmStatusNotificationAsync(message);
         });
      }

      public async Task<AlgorithmParameter[]> GetAlgorithmParametersAsync(int strategyId)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _parameterResults = await strategyClient.GetAlgorithmParametersAsync(strategyId);
         });
         return _parameterResults;
      }

      public async Task<AlgorithmInstance> UpdateAlgorithmInstanceAsync(AlgorithmInstance instance)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _algorithmInstanceResult = await strategyClient.UpdateAlgorithmInstanceAsync(instance);
         });
         return _algorithmInstanceResult;
      }

      public async Task<AlgorithmMessage> UpdateAlgorithmMessageAsync(AlgorithmMessage message)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _algorithmMessageResult = await strategyClient.UpdateAlgorithmMessageAsync(message);
         });
         return _algorithmMessageResult;
      }

      public async Task<StrategyTransaction> GetStrategyTransactionAsync(int transactionId)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _transactionResult = await strategyClient.GetStrategyTransactionAsync(transactionId);
         });
         return _transactionResult;
      }

      public async Task<StrategyTransaction> SaveStrategyTransactionAsync(StrategyTransaction transaction, string brokerName)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _transactionResult = await strategyClient.SaveStrategyTransactionAsync(transaction, brokerName);
         });
         return _transactionResult;
      }

      public async Task<StrategyTransaction> UpdateStrategyTransactionAsync(StrategyTransaction transaction)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _transactionResult = await strategyClient.UpdateStrategyTransactionAsync(transaction);
         });
         return _transactionResult;
      }

      public async Task<StrategyTransaction[]> GetStrategyTransactionsAsync(int? count, bool? descending)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _transactionResults = await strategyClient.GetStrategyTransactionsAsync(count, descending);
         });
         return _transactionResults;
      }

      public async Task<StrategyTransaction[]> GetStrategyTransactionsCollectionAsync(int entryStrategyTransactionId)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _transactionResults = await strategyClient.GetStrategyTransactionsCollectionAsync(entryStrategyTransactionId);
         });
         return _transactionResults;
      }

      public async Task<StrategyTransaction[]> GetStrategyTransactionsByDateRangeAsync(DateTime dateBottom, DateTime? dateTop, int maxTransactions)
      {
         await WithStrategyClientAsync(async strategyClient =>
         {
            _transactionResults = await strategyClient.GetStrategyTransactionsByDateRangeAsync(dateBottom, dateTop, maxTransactions);
         });
         return _transactionResults;
      }
      #endregion

      #region Utilities
      protected virtual void WithStrategyClient(Action<IStrategyService> codeToExecute)
      {
         WithClient<IStrategyService>(_serviceFactory.CreateClient<IStrategyService>(), codeToExecute);
      }
      protected virtual async Task WithStrategyClientAsync(Func<IStrategyService, Task> codeToExecute)
      {
         await WithClientAsync<IStrategyService>(_serviceFactory.CreateClient<IStrategyService>(), codeToExecute);
      }
      #endregion
   }
}
