using N.Core.Business.Contracts;

namespace MarketMiner.Business.Contracts
{
   public interface IStrategyEngine : IBusinessEngine
   {
      void PostAlgorithmStatusNotification(string message);
      bool CanStrategyAcceptFunds(int strategyId, decimal amount);
      bool IsStrategyAlgorithmRunning(int strategyId);
   }
}
