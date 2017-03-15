using MarketMiner.Business.Entities;
using System.Collections.Generic;

namespace MarketMiner.Data.Contracts
{
   public class AccountSubscriptionInfo
   {
      public Account Account { get; set; }
      public Subscription Subscription { get; set; }
   }
}
