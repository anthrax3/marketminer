using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using P.Core.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(ICommunicationTemplateRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class CommunicationTemplateRepository : MarketMinerRepositoryBase<CommunicationTemplate>, ICommunicationTemplateRepository
   {
      #region Members.Override

      protected override CommunicationTemplate AddEntity(MarketMinerContext entityContext, CommunicationTemplate entity)
      {
         return entityContext.CommunicationTemplateSet.Add(entity);
      }

      protected override CommunicationTemplate UpdateEntity(MarketMinerContext entityContext, CommunicationTemplate entity)
      {
         return (from e in entityContext.CommunicationTemplateSet
                 where e.CommunicationTemplateID == entity.CommunicationTemplateID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<CommunicationTemplate> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.CommunicationTemplateSet
                select e;
      }

      protected override CommunicationTemplate GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.CommunicationTemplateSet
                      where e.CommunicationTemplateID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.ICommunicationTemplateRepository

      public IEnumerable<CommunicationTemplate> GetCommunicationTemplateByName(string name)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from t in entityContext.CommunicationTemplateSet
                        where t.Name == name
                        select t;

            return query.ToFullyLoaded();
         }
      }
      #endregion
   }
}
