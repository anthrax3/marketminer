using EFCache;
using N.Core.Common.Core;
using P.Core.Common.Contracts;
using System.Data.Entity;
using System.Data.Entity.Core.Common;

namespace MarketMiner.Data.Configurations
{
   internal sealed class ContextConfiguration : DbConfiguration
   {
      public ContextConfiguration()
      {
         #region cache
         InMemoryCache cache = null;
         try
         {
            // this fails during a manual migration thus the catch
            // alternatives are:
            //    1) use auto-migrations
            //    2) add a static InMemoryCache and ref. MarketMiner.Data from the client
            cache = ObjectBase.Container.GetExportedValue<IDataCache>() as InMemoryCache;
         }
         catch
         {
            cache = new MarketMinerCache();
         }

         var transactionHandler = new CacheTransactionHandler(cache);
         var cachingPolicy = new MarketMinerCachingPolicy();
         AddInterceptor(transactionHandler);
         Loaded += (sender, args) => args.ReplaceService<DbProviderServices>((s, _) => new CachingProviderServices(s, transactionHandler, cachingPolicy));
         #endregion
      }
   }
}
