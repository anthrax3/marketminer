using MarketMiner.Algorithm.Common;
using P.MarketMiner.Client.Entities;
using System.Threading.Tasks;

namespace MarketMiner.Algorithm.OANDA.Currencies
{
   [AlgorithmModule(Name = "AUDUSDAlgorithm")]
   public class AUDUSDAlgorithm : AlgorithmBase
   {
      public AUDUSDAlgorithm(int accountId, Strategy strategy)
         : base(strategy)
      {
      }

      public override Task<bool> Start()
      {
         // DoThisBeforeARateDecision();
         // DoThisAfterARateDecision();

         return base.Start();
      }
   }
}
