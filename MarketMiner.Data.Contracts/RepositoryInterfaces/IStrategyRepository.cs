using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IStrategyRepository : IDataRepository<Strategy>
   {
      Strategy GetStrategy(int strategyId);
      Strategy GetStrategyByName(string strategyName);
      IEnumerable<Strategy> GetStrategies();
      IEnumerable<Strategy> GetStrategiesByAlgorithmStatus(short algorithmStatus);
   }
}
