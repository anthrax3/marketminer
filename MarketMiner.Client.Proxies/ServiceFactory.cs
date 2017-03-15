using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System.ComponentModel.Composition;

namespace MarketMiner.Client.Proxies
{
   [Export(typeof(IServiceFactory))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ServiceFactory : IServiceFactory
   {
      T IServiceFactory.CreateClient<T>()
      {
         return ObjectBase.Container.GetExportedValue<T>();
      }
   }
}
