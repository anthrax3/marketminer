using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IAlgorithmParameterRepository : IDataRepository<AlgorithmParameter>
   {
      IEnumerable<AlgorithmParameter> GetAlgorithmParametersByStrategy(int strategyId);
   }
}
