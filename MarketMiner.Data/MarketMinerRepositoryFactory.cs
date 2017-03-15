using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System.ComponentModel.Composition;

namespace MarketMiner.Data
{
   [Export(typeof(IDataRepositoryFactory))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class MarketMinerRepositoryFactory : IDataRepositoryFactory
   {
      T IDataRepositoryFactory.GetDataRepository<T>()
      {
         return ObjectBase.Container.GetExportedValue<T>();
      }
   }
}
