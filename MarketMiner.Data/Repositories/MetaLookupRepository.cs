using MarketMiner.Data.Contracts;
using P.Core.Common.Meta;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IMetaLookupRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class MetaLookupRepository : MarketMinerRepositoryBase<MetaLookup>, IMetaLookupRepository
   {
      #region Members.Override
      protected override MetaLookup AddEntity(MarketMinerContext entityContext, MetaLookup entity)
      {
         return entityContext.MetaLookupSet.Add(entity);
      }

      protected override MetaLookup UpdateEntity(MarketMinerContext entityContext, MetaLookup entity)
      {
         return (from e in entityContext.MetaLookupSet
                 where e.MetaLookupID == entity.MetaLookupID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<MetaLookup> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.MetaLookupSet
                select e;
      }

      protected override MetaLookup GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.MetaLookupSet
                      where e.MetaLookupID == id
                      select e);

         var result = query.FirstOrDefault();

         return result;
      }
      #endregion

      #region Members.IMetaLookupRepository
      public IEnumerable<MetaLookup> GetLookups(string type, bool enabledOnly = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var lookups = GetEntities(entityContext).Where(x => x.Type == type);

            return lookups;
         }
      }

      public MetaLookup GetLookup(string type, string code, bool enabled = true)
      {
         IEnumerable<MetaLookup> lookups = GetLookups(type, false);

         MetaLookup lookup = lookups.FirstOrDefault(l => l.Code == code && l.Enabled == enabled);

         return lookup;
      }
      #endregion
   }
}
