using MarketMiner.Client.Contracts;
using MarketMiner.Client.Entities;
using System;
using System.Threading.Tasks;

namespace MarketMiner.Client.Proxies.ServiceCallers
{
   public class SubscriptionCaller : ServiceCaller
   {
      public static SubscriptionCaller Instance() { return new SubscriptionCaller(); }

      #region Operation result types
      Signal _signalResult;
      Signal[] _signalResults;
      Product _productResult;
      Product[] _productResults;
      #endregion

      #region Operations
      public Signal GeSignal(int signalId)
      {
         WithSubscriptionClient(subscriptionClient =>
         {
            _signalResult = subscriptionClient.GetSignal(signalId);
         });
         return _signalResult;
      }

      public Signal[] GetActiveSignalsByType(string type)
      {
         WithSubscriptionClient(subscriptionClient =>
         {
            _signalResults = subscriptionClient.GetActiveSignalsByType(type);
         });
         return _signalResults;
      }

      public Signal UpdateSignal(Signal signal)
      {
         WithSubscriptionClient(subscriptionClient =>
         {
            _signalResult = subscriptionClient.UpdateSignal(signal);
         });
         return _signalResult;
      }

      public Signal PushSignalToSubscribers(Signal signal)
      {
         WithSubscriptionClient(subscriptionClient =>
         {
            _signalResult = subscriptionClient.PushSignalToSubscribers(signal);
         });
         return _signalResult;
      }

      public Product[] GetProducts(bool activesOnly)
      {
         WithSubscriptionClient(subscriptionClient =>
         {
            _productResults = subscriptionClient.GetProducts(activesOnly);
         });
         return _productResults;
      }

      public Product GetProductByName(string name)
      {
         WithSubscriptionClient(subscriptionClient =>
         {
            _productResult = subscriptionClient.GetProductByName(name);
         });
         return _productResult;
      }
      #endregion

      #region Operations.Async
      public async Task<Signal> GetSignalAsync(int signalId)
      {
         await WithSubscriptionClientAsync(async subscriptionClient =>
         {
            _signalResult = await subscriptionClient.GetSignalAsync(signalId);
         });
         return _signalResult;
      }

      public async Task<Signal[]> GetActiveSignalsByTypeAsync(string type)
      {
         await WithSubscriptionClientAsync(async subscriptionClient =>
         {
            _signalResults = await subscriptionClient.GetActiveSignalsByTypeAsync(type);
         });
         return _signalResults;
      }

      public async Task<Signal> UpdateSignalAsync(Signal signal)
      {
         await WithSubscriptionClientAsync(async subscriptionClient =>
         {
            _signalResult = await subscriptionClient.UpdateSignalAsync(signal);
         });
         return _signalResult;
      }

      public async Task<Signal> PushSignalToSubscribersAsync(Signal signal)
      {
         await WithSubscriptionClientAsync(async subscriptionClient =>
         {
            _signalResult = await subscriptionClient.PushSignalToSubscribersAsync(signal);
         });
         return _signalResult;
      }
      #endregion

      #region Utilities
      protected virtual void WithSubscriptionClient(Action<ISubscriptionService> codeToExecute)
      {
         WithClient<ISubscriptionService>(_serviceFactory.CreateClient<ISubscriptionService>(), codeToExecute);
      }

      protected virtual async Task WithSubscriptionClientAsync(Func<ISubscriptionService, Task> codeToExecute)
      {
         await WithClientAsync<ISubscriptionService>(_serviceFactory.CreateClient<ISubscriptionService>(), codeToExecute);
      }
      #endregion
   }
}
