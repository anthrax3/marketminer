using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IFundRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class FundRepository : MarketMinerRepositoryBase<Fund>, IFundRepository
   {
      #region Members.Override
      protected override Fund AddEntity(MarketMinerContext entityContext, Fund entity)
      {
         return entityContext.FundSet.Add(entity);
      }

      protected override Fund UpdateEntity(MarketMinerContext entityContext, Fund entity)
      {
         return (from e in entityContext.FundSet
                 where e.FundID == entity.FundID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Fund> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.FundSet
                select e;
      }

      protected override Fund GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.FundSet
                      where e.FundID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IFundRepository
      public IEnumerable<Fund> GetFunds()
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return GetEntities(entityContext);
         }
      }

      public IEnumerable<Fund> GetFundsByStrategy(int strategyId)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from f in entityContext.FundSet
                        join s in entityContext.StrategySet on f.FundID equals s.FundID
                        select f;

            return query;
         }
      }

      public IEnumerable<Fund> GetFundsByAccount(int accountId)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from a in entityContext.AccountSet
                        join p in entityContext.ParticipationSet on a.AccountID equals p.AccountID
                        join f in entityContext.FundSet on p.FundID equals f.FundID
                        where a.AccountID == accountId
                        select f;

            return query;
         }
      }
      #endregion
   }
}
