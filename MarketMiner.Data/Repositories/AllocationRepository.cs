using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using P.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IAllocationRepository))]
   [PartCreationPolicy(CreationPolicy.NonShared)]
   public class AllocationRepository : MarketMinerRepositoryBase<Allocation>, IAllocationRepository
   {
      #region Members.Override

      protected override Allocation AddEntity(MarketMinerContext entityContext, Allocation entity)
      {
         return entityContext.AllocationSet.Add(entity);
      }

      protected override Allocation UpdateEntity(MarketMinerContext entityContext, Allocation entity)
      {
         return (from e in entityContext.AllocationSet
                 where e.AllocationID == entity.AllocationID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Allocation> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.AllocationSet
                select e;
      }

      protected override Allocation GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.AllocationSet
                      where e.AllocationID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IAllocationRepository

      public IEnumerable<Allocation> GetAllocationsByDate(DateTime dateCreated)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from r in entityContext.AllocationSet
                        where r.DateCreated == dateCreated
                        select r;

            return query.ToFullyLoaded();
         }
      }

      public IEnumerable<Allocation> GetAllocationsByDateRange(DateTime dateBottom, DateTime dateTop)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from r in entityContext.AllocationSet
                        where r.DateCreated >= dateBottom && r.DateCreated <= dateTop
                        select r;

            return query.ToFullyLoaded();
         }
      }
      #endregion
   }
}
