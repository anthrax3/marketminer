using MarketMiner.Data.Contracts;
using P.Core.Common.Meta;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using P.Core.Common.Extensions;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IMetaResourceRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class MetaResourceRepository : MarketMinerRepositoryBase<MetaResource>, IMetaResourceRepository
   {
      #region Members.Override
      protected override MetaResource AddEntity(MarketMinerContext entityContext, MetaResource entity)
      {
         return entityContext.MetaResourceSet.Add(entity);
      }

      protected override MetaResource UpdateEntity(MarketMinerContext entityContext, MetaResource entity)
      {
         return (from e in entityContext.MetaResourceSet
                 where e.MetaResourceID == entity.MetaResourceID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<MetaResource> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.MetaResourceSet
                select e;
      }

      protected override MetaResource GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.MetaResourceSet
                      where e.MetaResourceID == id
                      select e);

         var result = query.FirstOrDefault();

         return result;
      }
      #endregion

      #region Members.IMetaResourceRepository
      public MetaResource GetResource(string set, string type, string key, string cultureCode = "en-US", bool enabled = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var resource = GetEntities(entityContext)
               .Where(x => x.Set == set && x.Type == type && x.Key == key && x.CultureCode == cultureCode && x.Enabled == enabled)
               .FirstOrDefault();

            return resource;
         }
      }

      public IEnumerable<MetaResource> GetResources(string set, bool enabledOnly = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var resources = GetEntities(entityContext).Where(x => x.Set == set);

            if (enabledOnly) resources.Where(r => r.Enabled);

            return resources;
         }
      }

      public IEnumerable<MetaResource> GetResources(string set, string type, bool enabledOnly = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var resources = GetEntities(entityContext).Where(x => x.Set == set && x.Type == type);

            if (enabledOnly) resources.Where(r => r.Enabled);

            return resources;
         }
      }

      public IEnumerable<MetaResource> GetResourcesByCulture(string set, string cultureCode = "en-US", bool enabledOnly = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var resources = GetEntities(entityContext).Where(x => x.Set == set && x.CultureCode == cultureCode);

            if (enabledOnly) resources.Where(r => r.Enabled);

            return resources;
         }
      }

      public IEnumerable<MetaResource> GetResourcesByCulture(string set, string type, string cultureCode = "en-US", bool enabledOnly = true)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var resources = GetEntities(entityContext).Where(x => x.Set == set && x.Type == type && x.CultureCode == cultureCode);

            if (enabledOnly) resources.Where(r => r.Enabled);

            return resources;
         }
      }
      #endregion
   }
}
