using MarketMiner.Client.Entities;
using MarketMiner.Common.Faults;
using P.Core.Common.Contracts;
using P.Core.Common.Faults;
using System.ServiceModel;
using System.Threading.Tasks;

namespace MarketMiner.Client.Contracts
{
   [ServiceContract]
   public interface ISubscriptionService : IServiceContract
   {
      #region Operations
      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Subscription[] GetSubscriptions();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Subscription[] GetSubscriptionsByAccount(int accountId);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      Subscription UpdateSubscription(Subscription subscription);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Signal[] GetSignals();

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Signal GetSignal(int signalId);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Signal[] GetActiveSignalsByType(string type);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      Signal UpdateSignal(Signal signal);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      Signal PushSignalToSubscribers(Signal signal);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Product[] GetProducts(bool activesOnly = true);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Product GetProductByCode(string code);

      [OperationContract]
      [FaultContract(typeof(NotFoundFault))]
      [FaultContract(typeof(AuthorizationValidationFault))]
      Product GetProductByName(string name);

      [OperationContract]
      [FaultContract(typeof(EntityValidationFault))]
      [TransactionFlow(TransactionFlowOption.Allowed)]
      Product UpdateProduct(Product product);
      #endregion

      #region Operations.Async
      [OperationContract]
      Task<Subscription[]> GetSubscriptionsAsync();

      [OperationContract]
      Task<Subscription[]> GetSubscriptionsByAccountAsync(int accountId);

      [OperationContract]
      Task<Subscription> UpdateSubscriptionAsync(Subscription subscription);

      [OperationContract]
      Task<Signal> GetSignalAsync(int signalId);

      [OperationContract]
      Task<Signal[]> GetSignalsAsync();

      [OperationContract]
      Task<Signal[]> GetActiveSignalsByTypeAsync(string type);

      [OperationContract]
      Task<Signal> UpdateSignalAsync(Signal signal);

      [OperationContract]
      Task<Signal> PushSignalToSubscribersAsync(Signal signal);

      [OperationContract]
      Task<Product[]> GetProductsAsync(bool activesOnly = true);

      [OperationContract]
      Task<Product> GetProductByCodeAsync(string code);

      [OperationContract]
      Task<Product> GetProductByNameAsync(string name);

      [OperationContract]
      Task<Product> UpdateProductAsync(Product product);
      #endregion
   }
}
