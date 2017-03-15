using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IFundRepository : IDataRepository<Fund>
   {
      IEnumerable<Fund> GetFunds();
      IEnumerable<Fund> GetFundsByStrategy(int strategyId);
      IEnumerable<Fund> GetFundsByAccount(int accountId);
   }
}
