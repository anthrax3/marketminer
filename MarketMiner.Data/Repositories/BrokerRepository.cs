using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IBrokerRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class BrokerRepository : MarketMinerRepositoryBase<Broker>, IBrokerRepository
   {
      #region Members.Override
      protected override Broker AddEntity(MarketMinerContext entityContext, Broker entity)
      {
         return entityContext.BrokerSet.Add(entity);
      }

      protected override Broker UpdateEntity(MarketMinerContext entityContext, Broker entity)
      {
         return (from e in entityContext.BrokerSet
                 where e.BrokerID == entity.BrokerID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Broker> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.BrokerSet
                select e;
      }

      protected override Broker GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.BrokerSet
                      where e.BrokerID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IBrokerRepository
      public Broker GetBrokerByName(string name)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var broker = GetEntities(entityContext).Where(x => x.Name == name).FirstOrDefault();

            return broker;
         }
      }
      #endregion
   }
}
