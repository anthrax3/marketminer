using MarketMiner.Business.Entities;
using P.Core.Common.Contracts;

namespace MarketMiner.Data.Contracts
{
   public interface IAccountRepository : IDataRepository<Account>
   {
      Account GetByLogin(string login);
      Account GetById(int accountId);
   }
}
