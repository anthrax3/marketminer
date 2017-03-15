using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface IAlgorithmMessageRepository : IDataRepository<AlgorithmMessage>
   {
      IEnumerable<AlgorithmMessage> GetAlgorithmMessagesByInstance(int instanceId);
      IEnumerable<AlgorithmMessage> GetAlgorithmMessagesByStrategy(int strategyId);
   }
}
