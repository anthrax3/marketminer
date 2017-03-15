using EFCache;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace MarketMiner.Data
{
   [Export(typeof(IDataCache))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class MarketMinerCache : InMemoryCache, IDataCache
   {
      public void AddItem(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTimeOffset absoluteExpiration)
      {
         PutItem(key, value, dependentEntitySets, slidingExpiration, absoluteExpiration);
      }

      public void RemoveItem(string key)
      {
         InvalidateItem(key);
      }

      public void RemoveSets(IEnumerable<string> entitySets)
      {
         InvalidateSets(entitySets);
      }
   }
}
