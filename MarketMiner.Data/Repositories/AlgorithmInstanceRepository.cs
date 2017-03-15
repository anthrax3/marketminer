using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IAlgorithmInstanceRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class AlgorithmInstanceRepository : MarketMinerRepositoryBase<AlgorithmInstance>, IAlgorithmInstanceRepository
   {
      #region Members.Override
      protected override AlgorithmInstance AddEntity(MarketMinerContext entityContext, AlgorithmInstance entity)
      {
         return entityContext.AlgorithmInstanceSet.Add(entity);
      }

      protected override AlgorithmInstance UpdateEntity(MarketMinerContext entityContext, AlgorithmInstance entity)
      {
         return (from e in entityContext.AlgorithmInstanceSet
                 where e.AlgorithmInstanceID == entity.AlgorithmInstanceID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<AlgorithmInstance> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.AlgorithmInstanceSet
                select e;
      }

      protected override AlgorithmInstance GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.AlgorithmInstanceSet
                      where e.AlgorithmInstanceID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IAlgorithmInstanceRepository
      public IEnumerable<AlgorithmInstance> GetAlgorithmInstances()
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return GetEntities(entityContext);
         }
      }

      public IEnumerable<AlgorithmInstance> GetAlgorithmInstancesByStrategy(int strategyId)
      {
         return GetAlgorithmInstances().Where(a => a.StrategyID == strategyId);
      }

      public IEnumerable<AlgorithmInstance> GetAlgorithmInstancesByStatus(short algorithmStatus)
      {
         return GetAlgorithmInstances().Where(a => a.Status == algorithmStatus);
      }
      #endregion
   }
}
