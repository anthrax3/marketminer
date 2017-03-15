using P.Core.Common.Contracts;
using MarketMiner.Business.Entities;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public interface ISubscriptionRepository : IDataRepository<Subscription>
   {
      IEnumerable<Subscription> GetSubscriptionsByInstrument(string instrument);
      IEnumerable<AccountSubscriptionInfo> GetAccountSubscriptionsByProduct(int productId);
      IEnumerable<AccountSubscriptionInfo> GetCurrentAccountSubscriptionInfo();
      IEnumerable<AccountSubscriptionInfo> GetAccountOpenSubscriptionInfo(int accountId);
   }
}
