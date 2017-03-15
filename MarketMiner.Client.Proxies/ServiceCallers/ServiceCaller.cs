using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace MarketMiner.Client.Proxies
{
   public abstract class ServiceCaller
   {
      protected IServiceFactory _serviceFactory = ObjectBase.Container.GetExportedValue<IServiceFactory>();

      protected virtual void WithClient<T>(Action<T> codeToExecute) where T : IServiceContract
      {
         T proxy = _serviceFactory.CreateClient<T>();

         WithClient<T>(proxy, codeToExecute);
      }

      protected virtual void WithClient<T>(T proxy, Action<T> codeToExecute)
      {
         // execute service call using the proxy
         codeToExecute.Invoke(proxy);
         DisposeProxy((IDisposable)proxy);
      }

      protected virtual async Task WithClientAsync<T>(T proxy, Func<T, Task> codeToExecute)
      {
         await codeToExecute.Invoke(proxy);
         DisposeProxy((IDisposable)proxy);
      }

      void DisposeProxy(IDisposable proxy)
      {
         // dispose the proxy
         IDisposable disposableClient = proxy as IDisposable;
         if (disposableClient != null)
            disposableClient.Dispose();
      }
   }
}
