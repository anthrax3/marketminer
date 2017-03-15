using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IStrategyTransactionRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class StrategyTransactionRepository : MarketMinerRepositoryBase<StrategyTransaction>, IStrategyTransactionRepository
   {
      #region Members.Override
      protected override StrategyTransaction AddEntity(MarketMinerContext entityContext, StrategyTransaction entity)
      {
         return entityContext.StrategyTransactionSet.Add(entity);
      }

      protected override StrategyTransaction UpdateEntity(MarketMinerContext entityContext, StrategyTransaction entity)
      {
         return (from e in entityContext.StrategyTransactionSet
                 where e.StrategyTransactionID == entity.StrategyTransactionID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<StrategyTransaction> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.StrategyTransactionSet
                select e;
      }

      protected override StrategyTransaction GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.StrategyTransactionSet
                      where e.StrategyTransactionID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IStrategyTransactionRepository
      public IEnumerable<StrategyTransaction> GetStrategyTransactions()
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return GetEntities(entityContext);
         }
      }

      public IEnumerable<StrategyTransaction> GetStrategyTransactionsByBroker(int brokerId)
      {
         return GetStrategyTransactions().Where(t => t.BrokerID == brokerId);
      }

      public IEnumerable<StrategyTransaction> GetStrategyTransactionsByInstrument(string instrument)
      {
         return GetStrategyTransactions().Where(t => t.Instrument == instrument);
      }

      public IEnumerable<StrategyTransaction> GetStrategyTransactionsBySide(string side)
      {
         return GetStrategyTransactions().Where(t => t.Side == side);
      }

      public IEnumerable<StrategyTransaction> GetStrategyTransactionsByDate(DateTime dateCreated)
      {
         return GetStrategyTransactions().Where(t => t.DateCreated == dateCreated);
      }

      public IEnumerable<StrategyTransaction> GetStrategyTransactionsByDateRange(DateTime dateBottom, DateTime dateTop)
      {
         return GetStrategyTransactions().Where(t => dateBottom >= t.DateCreated && dateTop <= t.DateCreated);
      }
      #endregion
   }
}
