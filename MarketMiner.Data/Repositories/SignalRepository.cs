using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(ISignalRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class SignalRepository : MarketMinerRepositoryBase<Signal>, ISignalRepository
   {
      #region Members.Override
      protected override Signal AddEntity(MarketMinerContext entityContext, Signal entity)
      {
         return entityContext.SignalSet.Add(entity);
      }

      protected override Signal UpdateEntity(MarketMinerContext entityContext, Signal entity)
      {
         return (from e in entityContext.SignalSet
                 where e.SignalID == entity.SignalID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Signal> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.SignalSet
                select e;
      }

      protected override Signal GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.SignalSet
                      where e.SignalID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.ISignalRepository
      public IEnumerable<Signal> GetSignals(bool activesOnly = false)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            if (activesOnly)
               return GetEntities(entityContext).Where(e => e.Active == true).ToArray().ToList();
            else
               return GetEntities(entityContext).ToArray().ToList();
         }
      }

      public IEnumerable<Signal> GetSignalsByType(string type, bool activesOnly = false)
      {
         return GetSignals(activesOnly).Where(e => e.Type == type);
      }

      public IEnumerable<Signal> GetSignalsByInstrument(string instrument, bool activesOnly = false)
      {
         return GetSignals(activesOnly).Where(e => e.Instrument == instrument);
      }

      public IEnumerable<Signal> GetSignalsByGranularity(string granularity, bool activesOnly = false)
      {
         return GetSignals(activesOnly).Where(e => e.Granularity == granularity);
      }

      public IEnumerable<Signal> GetSignalsByDate(DateTime dateCreated)
      {
         return GetSignals().Where(e => e.DateCreated == dateCreated);
      }

      public IEnumerable<Signal> GetSignalsByDateRange(DateTime dateBottom, DateTime dateTop)
      {
         return GetSignals().Where(e => e.DateCreated >= dateBottom && e.DateCreated <= dateTop);
      }
      #endregion
   }
}
