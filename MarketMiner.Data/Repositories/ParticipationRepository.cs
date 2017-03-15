using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using P.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IParticipationRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class ParticipationRepository : MarketMinerRepositoryBase<Participation>, IParticipationRepository
   {
      #region Members.Override

      protected override Participation AddEntity(MarketMinerContext entityContext, Participation entity)
      {
         return entityContext.ParticipationSet.Add(entity);
      }

      protected override Participation UpdateEntity(MarketMinerContext entityContext, Participation entity)
      {
         return (from e in entityContext.ParticipationSet
                 where e.ParticipationID == entity.ParticipationID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Participation> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.ParticipationSet
                select e;
      }

      protected override Participation GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.ParticipationSet
                      where e.ParticipationID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      //---------------------------------------------------------------------------------------------------
      #region Members.IParticipationRepository
      public IEnumerable<Participation> GetParticipationsByAccount(int accountId)
      {
         return GetParticipations().Where(p => p.Account.AccountID == accountId);
      }

      public IEnumerable<Participation> GetParticipations()
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return GetEntities(entityContext);
         }
      }

      public IEnumerable<Participation> GetParticipationsByDate(DateTime dateCreated)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from r in entityContext.ParticipationSet
                        where r.DateCreated == dateCreated
                        select r;

            return query.ToFullyLoaded();
         }
      }

      public IEnumerable<Participation> GetParticipationsByDateCreated(DateTime dateRange)
      {
         return GetParticipationsByDateCreatedRange(dateRange, dateRange);
      }

      public IEnumerable<Participation> GetParticipationsByDateCreatedRange(DateTime dateBottom, DateTime dateTop)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from p in entityContext.ParticipationSet
                        where p.DateCreated >= dateBottom && p.DateCreated <= dateTop
                        select p;

            return query.ToFullyLoaded();
         }
      }

      public IEnumerable<Participation> GetParticipationsByStrategy(short strategyId)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from p in entityContext.ParticipationSet
                        where p.Fund.Strategies.FirstOrDefault(s => s.StrategyID == strategyId) != null
                        select p;

            return query.ToFullyLoaded();
         }
      }
      #endregion
   }
}
