using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface ICommunicationRepository : IDataRepository<Communication>
   {
      IEnumerable<Communication> GetCommunicationsByDate(DateTime dateCreated);
      IEnumerable<Communication> GetCommunicationsByDateRange(DateTime dateBottom, DateTime dateTop);
   }
}
