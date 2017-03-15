using P.Core.Common.Contracts;
using MarketMiner.Business.Entities;
using System;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IReservationRepository : IDataRepository<Reservation>
   {
      IEnumerable<Reservation> GetOpenReservationsByAccount(int accountId);
      IEnumerable<Reservation> GetReservationsByDate(DateTime dateCreated);
      IEnumerable<Reservation> GetReservationsByDateRange(DateTime dateBottom, DateTime dateTop);
   }
}
