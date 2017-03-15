using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using P.Core.Common.Extensions;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IAlgorithmParameterRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class AlgorithmParameterRepository : MarketMinerRepositoryBase<AlgorithmParameter>, IAlgorithmParameterRepository
   {
      #region Members.Override
      protected override AlgorithmParameter AddEntity(MarketMinerContext entityContext, AlgorithmParameter entity)
      {
         return entityContext.AlgorithmParameterSet.Add(entity);
      }

      protected override AlgorithmParameter UpdateEntity(MarketMinerContext entityContext, AlgorithmParameter entity)
      {
         return (from e in entityContext.AlgorithmParameterSet
                 where e.AlgorithmParameterID == entity.AlgorithmParameterID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<AlgorithmParameter> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.AlgorithmParameterSet
                select e;
      }

      protected override AlgorithmParameter GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.AlgorithmParameterSet
                      where e.AlgorithmParameterID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IAlgorithmParameterRepository
      public IEnumerable<AlgorithmParameter> GetAlgorithmParameters()
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return GetEntities(entityContext).ToArray().ToList();
         }
      }

      public IEnumerable<AlgorithmParameter> GetAlgorithmParametersByStrategy(int strategyId)
      {
         return GetAlgorithmParameters().Where(a => a.StrategyID == strategyId);
      }
      #endregion
   }
}
