using N.Core.Business.Contracts;
using N.Core.Common.Core;
using System.ComponentModel.Composition;

namespace MarketMiner.Business
{
   [Export(typeof(IBusinessEngineFactory))]  // MefDI: interface mapping
   [PartCreationPolicy(CreationPolicy.NonShared)] // MEfDI: non-singleton
   public class BusinessEngineFactory : IBusinessEngineFactory
   {
      T IBusinessEngineFactory.GetBusinessEngine<T>()
      {
         return ObjectBase.Container.GetExportedValue<T>();
      }
   }
}
