using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IAlgorithmMessageRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class AlgorithmMessageRepository : MarketMinerRepositoryBase<AlgorithmMessage>, IAlgorithmMessageRepository
   {
      #region Members.Override
      protected override AlgorithmMessage AddEntity(MarketMinerContext entityContext, AlgorithmMessage entity)
      {
         return entityContext.AlgorithmMessageSet.Add(entity);
      }

      protected override AlgorithmMessage UpdateEntity(MarketMinerContext entityContext, AlgorithmMessage entity)
      {
         return (from e in entityContext.AlgorithmMessageSet
                 where e.AlgorithmMessageID == entity.AlgorithmMessageID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<AlgorithmMessage> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.AlgorithmMessageSet
                select e;
      }

      protected override AlgorithmMessage GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.AlgorithmMessageSet
                      where e.AlgorithmMessageID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IAlgorithmMessageRepository
      public IEnumerable<AlgorithmMessage> GetAlgorithmMessages()
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return GetEntities(entityContext);
         }
      }

      public IEnumerable<AlgorithmMessage> GetAlgorithmMessagesByInstance(int instanceId)
      {
         return GetAlgorithmMessages().Where(m => m.AlgorithmInstanceID == instanceId);
      }

      public IEnumerable<AlgorithmMessage> GetAlgorithmMessagesByStrategy(int strategyId)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from m in entityContext.AlgorithmMessageSet
                        join i in entityContext.AlgorithmInstanceSet on m.AlgorithmInstanceID equals i.AlgorithmInstanceID
                        where i.StrategyID == strategyId
                        select m;

            return query;
         }
      }
      #endregion
   }
}
