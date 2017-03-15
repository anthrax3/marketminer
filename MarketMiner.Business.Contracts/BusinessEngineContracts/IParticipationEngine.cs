using MarketMiner.Business.Entities;
using N.Core.Business.Contracts;

namespace MarketMiner.Business.Contracts
{
   public interface IParticipationEngine : IBusinessEngine
   {
      bool IsAccountParticipatingInFund(int accountId, int strategyId);
      bool CanFundAcceptNewParticipation(int fundId, decimal amount);
      bool CanFundAcceptParticipationAddition(int fundId, decimal amount);
      bool CanParticipationBeIncreased(int participationId, decimal amount);
      Participation PushAllocationIntoParticipation(Allocation allocation);
      Redemption RedeemFundsFromParticipation(int participationId, decimal amount);
   }
}
