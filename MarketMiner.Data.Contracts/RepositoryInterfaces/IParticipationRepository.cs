using P.Core.Common.Contracts;
using MarketMiner.Business.Entities;
using System;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IParticipationRepository : IDataRepository<Participation>
   {   
      IEnumerable<Participation> GetParticipations();
      IEnumerable<Participation> GetParticipationsByAccount(int accountId);
      IEnumerable<Participation> GetParticipationsByStrategy(short strategyId);
      IEnumerable<Participation> GetParticipationsByDateCreated(DateTime dateCreated);
      IEnumerable<Participation> GetParticipationsByDateCreatedRange(DateTime dateBottom, DateTime dateTop);
   }
}
