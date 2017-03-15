using MarketMiner.Business.Contracts;
using MarketMiner.Business.Entities;
using MarketMiner.Common;
using MarketMiner.Data.Contracts;
using N.Core.Business.Contracts;
using N.Core.Common.ServiceModel;
using P.Core.Common.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using MarketMiner.Business.Common;
using System;

namespace MarketMiner.Business.Managers
{
   [ApplyProxyDataContractResolver]
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, ReleaseServiceInstanceOnTransactionComplete = false)]
   public class SubscriptionManager : ManagerBase, ISubscriptionService
   {
      #region Constructors
      public SubscriptionManager() { }

      public SubscriptionManager(IDataRepositoryFactory dataRepositoryFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
      }

      public SubscriptionManager(IBusinessEngineFactory businessEngineFactory)
      {
         _BusinessEngineFactory = businessEngineFactory;
      }

      public SubscriptionManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
      {
         _DataRepositoryFactory = dataRepositoryFactory;
         _BusinessEngineFactory = businessEngineFactory;
      }
      #endregion

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Subscription[] GetSubscriptions()
      {
         return ExecuteFaultHandledOperation(() =>
         {
            ISubscriptionRepository subscriptionRepository = _DataRepositoryFactory.GetDataRepository<ISubscriptionRepository>();
            IEnumerable<Subscription> subscriptions = subscriptionRepository.Get();

            return subscriptions.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Subscription[] GetSubscriptionsByAccount(int accountId)
      {
         IEnumerable<Subscription> subscriptions = GetSubscriptions().Where(s => s.AccountID == accountId);

         if (subscriptions.Count() > 0)
            ValidateAuthorization(subscriptions.First());

         return subscriptions.ToArray();
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.User)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Subscription UpdateSubscription(Subscription subscription)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            ValidateAuthorization(subscription);

            ISubscriptionRepository subscriptionRepository = _DataRepositoryFactory.GetDataRepository<ISubscriptionRepository>();

            Subscription updatedEntity = null;

            if (subscription.SubscriptionID == 0)
               updatedEntity = subscriptionRepository.Add(subscription);
            else
               updatedEntity = subscriptionRepository.Update(subscription);

            return updatedEntity;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Signal[] GetSignals()
      {
         return ExecuteFaultHandledOperation(() =>
         {
            ISignalRepository signalRepository = _DataRepositoryFactory.GetDataRepository<ISignalRepository>();
            IEnumerable<Signal> signals = signalRepository.Get();

            return signals.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Signal GetSignal(int signalId)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            ISignalRepository signalRepository = _DataRepositoryFactory.GetDataRepository<ISignalRepository>();
            Signal signal = signalRepository.Get(signalId);

            return signal;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Signal[] GetActiveSignalsByType(string type)
      {
         return GetSignals().Where(s => s.Active && s.Type == type).ToArray();
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Signal UpdateSignal(Signal signal)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            ISignalRepository signalRepository = _DataRepositoryFactory.GetDataRepository<ISignalRepository>();

            Signal updatedEntity = null;

            if (signal.SignalID == 0)
               updatedEntity = signalRepository.Add(signal);
            else
               updatedEntity = signalRepository.Update(signal);

            return updatedEntity;
         });
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Signal PushSignalToSubscribers(Signal signal)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            Signal updatedEntity = UpdateSignal(signal);

            ISubscriptionEngine subscriptionEngine = _BusinessEngineFactory.GetBusinessEngine<ISubscriptionEngine>();

            Signal postMarkedEntity = subscriptionEngine.PushSignalToSubscribers(updatedEntity.SignalID);

            return postMarkedEntity;
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Product[] GetProducts(bool activesOnly = true)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IProductRepository productRepository = _DataRepositoryFactory.GetDataRepository<IProductRepository>();
            IEnumerable<Product> products = productRepository.GetProducts(activesOnly);

            return products.ToArray();
         });
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Product GetProductByCode(string code)
      {
         return GetProducts().FirstOrDefault(p => p.Code == code);
      }

      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Product GetProductByName(string name)
      {
         return GetProducts().FirstOrDefault(p => p.Name == name);
      }

      [OperationBehavior(TransactionScopeRequired = true)]
      [PrincipalPermission(SecurityAction.Demand, Role = OCOApp.Security.Admin)]
      public Product UpdateProduct(Product product)
      {
         return ExecuteFaultHandledOperation(() =>
         {
            IProductRepository productRepository = _DataRepositoryFactory.GetDataRepository<IProductRepository>();

            Product updatedEntity = null;

            if (product.ProductID == 0)
               updatedEntity = productRepository.Add(product);
            else
               updatedEntity = productRepository.Update(product);

            return updatedEntity;
         });
      }
   }
}
