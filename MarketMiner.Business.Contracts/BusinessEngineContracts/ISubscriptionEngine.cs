using MarketMiner.Business.Entities;
using N.Core.Business.Contracts;

namespace MarketMiner.Business.Contracts
{
   public interface ISubscriptionEngine : IBusinessEngine 
   {
      Signal PushSignalToSubscribers(int signalId);
   }
}
