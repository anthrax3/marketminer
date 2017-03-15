using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IAllocationRepository : IDataRepository<Allocation>
   {
      IEnumerable<Allocation> GetAllocationsByDate(DateTime dateCreated);
      IEnumerable<Allocation> GetAllocationsByDateRange(DateTime dateBottom, DateTime dateTop);
   }
}
