using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;

namespace MarketMiner.Data.Contracts
{
   public interface IBrokerRepository : IDataRepository<Broker>
   {
      Broker GetBrokerByName(string name);
   }
}
