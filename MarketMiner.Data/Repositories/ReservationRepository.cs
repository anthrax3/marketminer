using MarketMiner.Business.Entities;
using MarketMiner.Data.Contracts;
using P.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MarketMiner.Data.Repositories
{
   [Export(typeof(IReservationRepository))]  // MefDI: interface mapping
   [PartCreationPolicy(CreationPolicy.NonShared)] // MEfDI: non-singleton
   public class ReservationRepository : MarketMinerRepositoryBase<Reservation>, IReservationRepository
   {
      #region Members.Override

      protected override Reservation AddEntity(MarketMinerContext entityContext, Reservation entity)
      {
         return entityContext.ReservationSet.Add(entity);
      }

      protected override Reservation UpdateEntity(MarketMinerContext entityContext, Reservation entity)
      {
         return (from e in entityContext.ReservationSet
                 where e.ReservationID == entity.ReservationID
                 select e).FirstOrDefault();
      }

      protected override IEnumerable<Reservation> GetEntities(MarketMinerContext entityContext)
      {
         return from e in entityContext.ReservationSet
                select e;
      }

      protected override Reservation GetEntity(MarketMinerContext entityContext, int id)
      {
         var query = (from e in entityContext.ReservationSet
                      where e.ReservationID == id
                      select e);

         var results = query.FirstOrDefault();

         return results;
      }
      #endregion

      #region Members.IReservationRepository

      public IEnumerable<Reservation> GetOpenReservationsByAccount(int accountId)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            return GetEntities(entityContext).Where(r => r.Account.AccountID == accountId && r.Open == true);
         }
         
      }

      public IEnumerable<Reservation> GetReservationsByDate(DateTime dateCreated)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from r in entityContext.ReservationSet
                        where r.DateCreated == dateCreated
                        select r;

            return query.ToFullyLoaded();
         }
      }

      public IEnumerable<Reservation> GetReservationsByDateRange(DateTime dateBottom, DateTime dateTop)
      {
         using (MarketMinerContext entityContext = new MarketMinerContext())
         {
            var query = from r in entityContext.ReservationSet
                        where r.DateCreated >= dateBottom && r.DateCreated <= dateTop
                        select r;

            return query.ToFullyLoaded();
         }
      }
      #endregion
   }
}
