using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using P.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(ICommunicationRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class CommunicationRepository : MarketMinerRepositoryBase<Communication>, ICommunicationRepository
   {
      #region Members.Override

      protected override Communication AddEntity(MarketMinerContext entityContext, Communication entity)
      {
         return entityContext.CommunicationSet.Add(entity);
      }

      protected override Communication UpdateEntity(MarketMinerContext entityContext, Communication entity)
      {
         return (from e in entityContext.CommunicationSet
                 where e.CommunicationID == entity.CommunicationID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Communication> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.CommunicationSet
                select e;
      }

      protected override Communication GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.CommunicationSet
                      where e.CommunicationID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.ICommunicationRepository

      public IEnumerable<Communication> GetCommunicationsByDate(DateTime dateCreated)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from r in entityContext.CommunicationSet
                        where r.DateCreated == dateCreated
                        select r;

            return query.ToFullyLoaded();
         }
      }

      public IEnumerable<Communication> GetCommunicationsByDateRange(DateTime dateBottom, DateTime dateTop)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from r in entityContext.CommunicationSet
                        where r.DateCreated >= dateBottom && r.DateCreated <= dateTop
                        select r;

            return query.ToFullyLoaded();
         }
      }
      #endregion
   }
}
