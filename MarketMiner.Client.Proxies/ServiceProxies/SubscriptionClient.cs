using MarketMiner.Client.Contracts;
using MarketMiner.Client.Entities;
using N.Core.Common.ServiceModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace MarketMiner.Client.Proxies
{
   [Export(typeof(ISubscriptionService))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class SubscriptionClient : UserClientBase<ISubscriptionService>, ISubscriptionService
   {
      #region Operations
      public Subscription[] GetSubscriptions()
      {
         return Channel.GetSubscriptions();
      }

      public Subscription[] GetSubscriptionsByAccount(int accountId)
      {
         return Channel.GetSubscriptionsByAccount(accountId);
      }

      public Subscription UpdateSubscription(Subscription subscription)
      {
         return Channel.UpdateSubscription(subscription);
      }

      public Signal[] GetSignals()
      {
         return Channel.GetSignals();
      }

      public Signal GetSignal(int signalId)
      {
         return Channel.GetSignal(signalId);
      }

      public Signal[] GetActiveSignalsByType(string type)
      {
         return Channel.GetActiveSignalsByType(type);
      }

      public Signal UpdateSignal(Signal signal)
      {
         return Channel.UpdateSignal(signal);
      }

      public Signal PushSignalToSubscribers(Signal signal)
      {
         return Channel.PushSignalToSubscribers(signal);
      }

      public Product[] GetProducts(bool activesOnly = true)
      {
         return Channel.GetProducts(activesOnly);
      }

      public Product GetProductByCode(string code)
      {
         return Channel.GetProductByCode(code);
      }

      public Product GetProductByName(string name)
      {
         return Channel.GetProductByName(name);
      }

      public Product UpdateProduct(Product product)
      {
         return Channel.UpdateProduct(product);
      }
      #endregion

      #region Operations.Async
      public async Task<Subscription[]> GetSubscriptionsAsync()
      {
         return await Channel.GetSubscriptionsAsync();
      }

      public async Task<Subscription[]> GetSubscriptionsByAccountAsync(int accountId)
      {
         return await Channel.GetSubscriptionsByAccountAsync(accountId);
      }

      public async Task<Subscription> UpdateSubscriptionAsync(Subscription subscription)
      {
         return await Channel.UpdateSubscriptionAsync(subscription);
      }

      public async Task<Signal> GetSignalAsync(int signalId)
      {
         return await Channel.GetSignalAsync(signalId);
      }

      public async Task<Signal[]> GetSignalsAsync()
      {
         return await Channel.GetSignalsAsync();
      }

      public async Task<Signal[]> GetActiveSignalsByTypeAsync(string type)
      {
         return await Channel.GetActiveSignalsByTypeAsync(type);
      }

      public async Task<Signal> UpdateSignalAsync(Signal signal)
      {
         return await Channel.UpdateSignalAsync(signal);
      }

      public async Task<Signal> PushSignalToSubscribersAsync(Signal signal)
      {
         return await Channel.PushSignalToSubscribersAsync(signal);
      }

      public async Task<Product[]> GetProductsAsync(bool activesOnly = true)
      {
         return await Channel.GetProductsAsync(activesOnly);
      }

      public async Task<Product> GetProductByCodeAsync(string code)
      {
         return await Channel.GetProductByCodeAsync(code);
      }

      public async Task<Product> GetProductByNameAsync(string name)
      {
         return await Channel.GetProductByNameAsync(name);
      }

      public async Task<Product> UpdateProductAsync(Product product)
      {
         return await Channel.UpdateProductAsync(product);
      }
      #endregion
   }
}
