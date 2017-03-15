using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IAlgorithmInstanceRepository : IDataRepository<AlgorithmInstance>
   {
      IEnumerable<AlgorithmInstance> GetAlgorithmInstancesByStrategy(int strategyId);
      IEnumerable<AlgorithmInstance> GetAlgorithmInstancesByStatus(short algorithmStatus);
   }
}
