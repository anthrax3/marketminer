using EFCache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketMiner.Data
{
   internal sealed class MarketMinerCachingPolicy : CachingPolicy
   {
      protected override bool CanBeCached(System.Collections.ObjectModel.ReadOnlyCollection<System.Data.Entity.Core.Metadata.Edm.EntitySetBase> affectedEntitySets, string sql, IEnumerable<KeyValuePair<string, object>> parameters)
      {
         // oco: i think the logic below turns off caching except for the meta tables .. not sure that makes sense

         //string[] cachedSets = new [] { "MetaFieldDefinitionSet", "MetaLookupSet", "MetaResourceSet", "MetaSettingSet" };

         //if (affectedEntitySets.Any(set => cachedSets.Contains(set.Name)))
         //   return true;
         //else
         //   return false;
         
         return base.CanBeCached(affectedEntitySets, sql, parameters);
      }

      protected override void GetCacheableRows(System.Collections.ObjectModel.ReadOnlyCollection<System.Data.Entity.Core.Metadata.Edm.EntitySetBase> affectedEntitySets, out int minCacheableRows, out int maxCacheableRows)
      {
         // oco: need to take a look at this later as a way to manage very large caches/memory for large tables
         // for now just leave this as is.
         base.GetCacheableRows(affectedEntitySets, out minCacheableRows, out maxCacheableRows);
      }

      protected override void GetExpirationTimeout(System.Collections.ObjectModel.ReadOnlyCollection<System.Data.Entity.Core.Metadata.Edm.EntitySetBase> affectedEntitySets, out TimeSpan slidingExpiration, out DateTimeOffset absoluteExpiration)
      {
         base.GetExpirationTimeout(affectedEntitySets, out slidingExpiration, out absoluteExpiration);
      }
   }
}
