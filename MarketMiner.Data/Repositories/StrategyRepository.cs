using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IStrategyRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class StrategyRepository : MarketMinerRepositoryBase<Strategy>, IStrategyRepository
   {
      #region Members.Override
      protected override Strategy AddEntity(MarketMinerContext entityContext, Strategy entity)
      {
         return entityContext.StrategySet.Add(entity);
      }

      protected override Strategy UpdateEntity(MarketMinerContext entityContext, Strategy entity)
      {
         return (from e in entityContext.StrategySet
                 where e.StrategyID == entity.StrategyID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Strategy> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.StrategySet
                select e;
      }

      protected override Strategy GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.StrategySet
                      where e.StrategyID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IStrategyRepository
      public Strategy GetStrategy(int strategyId)
      {
         return GetStrategies().Where(e => e.StrategyID == strategyId).FirstOrDefault();
      }

      public Strategy GetStrategyByName(string strategyName)
      {
         return GetStrategies().Where(e => e.Name == strategyName).FirstOrDefault();
      }

      public IEnumerable<Strategy> GetStrategies()
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return GetEntities(entityContext);
         }
      }

      public IEnumerable<Strategy> GetStrategiesByAlgorithmStatus(short algorithmStatus)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from s in entityContext.StrategySet
               join a in entityContext.AlgorithmInstanceSet on s.StrategyID equals a.StrategyID
               where a.Status > 0
               select s;

            return query;
         }
      }
      #endregion
   }
}
